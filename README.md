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