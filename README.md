# auth-example-for-lee

This project was created with [Craftsman](https://github.com/pdevito3/craftsman).

## Getting Started
### Docker Setup
1. Run `docker-compose up --build` from your `.sln` directory to spin up your database(s) and other supporting 
infrastructure depending on your configuration (e.g. RabbitMQ, Keycloak, Jaeger, etc.).

### Keycloak Auth Server
1. If using a Keycloak auth server, you'll need to use the scaffolded Pulumi setup or configure it manually (new realm, client, etc) or .
   1. [Install the pulumi CLI](https://www.pulumi.com/docs/get-started/)
   1. `cd` to your the `KeycloackPulumi` project directory
   1. Run `pulumi up` to start the scaffolding process
   1. Create a new stack by pressing `Enter` when prompted and then typing the name of the stack (e.g. `dev`). Alternatively
      you can use the `pulumi stack init` command to make a new stack first.
      > Note: The stack name must match the extension on your yaml config file (e.g. `Pulumi.dev.yaml`) would have a stack of `dev`.
   1. Select yes to apply the configuration to your local Keycloak instance.
   1. Navigate to keycloak client at `localhost:3255/auth` and login with `admin` for username and password to view config (if you want). 

### Api
If you want to run your api: 
1. Make sure you have the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) installed 
2. Make sure you have entity framework installed: `dotnet tool install --global dotnet-ef`
3. Apply migrations
    1. Confirm your environment (`ASPNETCORE_ENVIRONMENT`) is set to `Development` using 
    `$Env:ASPNETCORE_ENVIRONMENT = "Development"` for powershell or `export ASPNETCORE_ENVIRONMENT=Development` for bash.
   2. `cd` to the boundary project root (e.g. `cd RecipeManagement/src/RecipeManagement`)
   3. Run `dotnet ef database update` to apply your migrations.
4. From the `RecipeManagement/src/RecipeManagement` directory, run the api: `dotnet run`
5. Go to  `https://localhost:5375/swagger/index.html` to use swagger

### Next JS
1. add an `.env.local` file to the root of your next project:
```
NEXTAUTH_URL=http://localhost:8582
NEXTAUTH_SECRET= 974d6f71-d41b-4601-9a7a-a33081f82188
```
2. `cd` to next app
2. Run `yarn` and then `yarn dev` 
2. Go to `http://localhost:8582/`
2. Press `Login`
3. Enter `alice` for username and password. You will be prompted to change your password, you can change it to whatever you want.
4. Use and explore 



## Summary of Gaps

### Session Logout ⭐️⭐️

Setup for providers is straightforward and I like that you can use a `well-known`, but you currently have to handroll your session signout with an event. If `next-auth` has a `well-known`, they should really be able to  get the `end_session_endpoint` and do it for me. Maybe backchannel notifications are the preffered way for the lib, but I'm not sure? [See here](https://youtu.be/UBFx3MSu1Rc?t=2841).

```tsx
 providers: [
    {
      id: "oidc",
      name: "OIDC",
      type: "oauth",
      wellKnown: `${env.clientUrls.authServer()}/.well-known/openid-configuration`,
      authorization: {
        params: { scope: "openid email profile recipe_management" },
      },
      idToken: true,
      checks: ["pkce", "state"],
      clientId: "recipe_management.next",
      clientSecret: env.auth.secret,
      profile(profile) {
        return {
          id: profile.sub,
          name: profile.name,
          email: profile.email,
          image: profile.picture,
        };
      },
    },
  ],
  events: {
    async signOut({ token }) {
      var refreshToken = token.refreshToken;
      let headers = { "Content-Type": "application/x-www-form-urlencoded" };
      try {
        await clients.authServer.post(
          `/protocol/openid-connect/logout`,
          querystring.stringify({
            refresh_token: refreshToken,
            client_secret: env.auth.secret,
            client_id: "recipe_management.next",
          }),
          { headers }
        );
      } catch (e) {}
    },
  },
```



### JWT Handling ⭐️⭐️

Like the above, needing to build a JWT like below shouldn't be something required out of the box. Even worse, it's worth noting that [the example in the docs](https://next-auth.js.org/tutorials/refresh-token-rotation) isn't even using a proper calculation. I belive it should be going off of the expiration in the access token to stay in sync with the server using the OAuth standard value of `exp` in the access token (more like the below, though for all i know i have gaps -- hand rolled!).

```tsx
    async jwt({ token, user, account }) {
      // Initial sign in
      if (account && user) {
        const decodedAccessToken = parseJwt(account.access_token);
        const nextAuthToken = {
          accessToken: account.access_token,
          accessTokenExpires: decodedAccessToken.exp * 1000,
          refreshToken: account.refresh_token,
          user,
        };
        return nextAuthToken;
      }

      // Return previous token if the access token has not expired yet
      if (Date.now() < Number(token.accessTokenExpires)) {
        return token;
      }

      // Access token has expired, try to update it
      return refreshAccessToken(token);
    },
    async session({ session, token }) {
      session.user = token.user as User;
      session.accessToken = token.accessToken;
      session.error = token.error;

      return session;
    },
  },

//....

function parseJwt(token) {
  return JSON.parse(Buffer.from(token.split(".")[1], "base64").toString());
}

```



### Calling Apis ⭐️⭐️

> ✉️ This is potentially fine as is (at least for the time being) if there's documentation on it but there is nothing about this on the site at all

This was a frustrating one for me. While NextJS apis are great and have great uses caes, even so, one of the primary reasons for adding auth to many business systems is presumably to be able to call api's throughout your business and be able to authenticate with those systems.

When using `next-auth`, it encodes the token when storing it and, as far as i know and have seen from resources that is great, but is not able to decode the token and proxy API calls to your api endpoints.

Currently, I need to get the token from my session and pass it as a `Bearer` for every call. Personally made an axios client like this to make it a bit easier, but feels wrong to me.

```tsx
import { env } from "@/utils/environmentService";
import Axios from "axios";
import { getSession, signOut } from "next-auth/react";

export const clients = {
  recipeManagement: (headers?: { [key: string]: string }) =>
    buildApiClient({
      baseURL: `${env.clientUrls.recipeManagement()}/api`,
      customHeaders: headers,
    }),
  authServer: Axios.create({
    baseURL: env.clientUrls.authServer(),
  }),
};

interface ApiClientProps {
  baseURL?: string;
  customHeaders?: {
    [key: string]: string;
  };
}

async function buildApiClient({ baseURL, customHeaders }: ApiClientProps) {
  var session = await getSession();
  var token = session?.accessToken;

  const client = Axios.create({
    baseURL,
    withCredentials: true,
    timeout: 30_000, // If you want to increase this, do it for a specific call, not the global app API.
    headers: {
      "X-CSRF": "1",
      Authorization: `Bearer ${token}`,
      ...customHeaders,
    },
  });

  client.interceptors.response.use(
    (response) => response,
    async (error) => {
      if (error.response) {
        // The request was made and the server responded with a status code
        // that falls out of the range of 2xx
        console.error(
          error.response.status,
          error.response.data,
          error.response.headers
        );
      } else if (error.request) {
        // The request was made but no response was received
        // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
        // http.ClientRequest in node.js
        console.error(error.request);
      }

      if (error && error.response && error.response.status === 401) {
        signOut();
      }
      console.log((error && error.toJSON && error.toJSON()) || undefined);

      return Promise.reject(error);
    }
  );

  return client;
}

```



#### Proposed Enhancement

Duende has [a solid method](https://docs.duendesoftware.com/identityserver/v5/bff/apis/remote/#use-our-built-in-simple-http-forwarder) for proxying enpoints using YARP. I'd propose a similar alternative witht this and the above documented. Guessing this might have to go in middleware, but this type of setup would add more control over your endpoints 

```tsx
export const authOptions: NextAuthOptions = {
  // https://next-auth.js.org/configuration/providers/oauth
  providers: [
    {
      id: "oidc",
      name: "OIDC",
      type: "oauth",
      wellKnown: `${env.clientUrls.authServer()}/.well-known/openid-configuration`,
      authorization: {
        params: { scope: "openid email profile recipe_management" },
      },
      idToken: true,
      checks: ["pkce", "state"],
      clientId: "recipe_management.next",
      clientSecret: env.auth.secret,
      profile(profile) {
        return {
          id: profile.sub,
          name: profile.name,
          email: profile.email,
          image: profile.picture,
        };
      },
    },
  ],
  events: {},
  callbacks: {},
  cookies: {},
  remoteEndpoints: {
    proxy(endpoints) {
    	endpoints.MapRemoteBffApiEndpoint("/api/customers", "https://remoteHost/customers")
        .RequireAccessToken(TokenType.User); // this could be `User` for code flow, `Client` for Client Credentials flow, or `TokenType.UserOrClient` for either.
	  }
  },
};

```



### Security Enhancements ⭐️

#### Anti-forgery protection

[Protect apis with a custom header](https://youtu.be/UBFx3MSu1Rc?t=3024). Could be anything, but having a custom header will force the browser to kick off a CORS preflight request. Duende enforces this with middleware, so maybe that's something that can be added in next middleware?



