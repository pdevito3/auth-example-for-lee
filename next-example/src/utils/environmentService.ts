const _env = process.env.NODE_ENV;
export const env = {
  environment: _env,
  isDevelopment: _env === "development",
  auth: {
    secret: "974d6f71-d41b-4601-9a7a-a33081f82188", // TODO change to env var ref
  },
  clientUrls: {
    recipeManagement: () => {
      switch (_env) {
        case "development":
          return "https://localhost:5375";
        default:
          throw "Environment variable not set for 'recipeManagement'";
      }
    },
    authServer: () => {
      switch (_env) {
        case "development":
          return "http://localhost:3255/auth/realms/DevRealm";
        default:
          throw "Environment variable not set for 'authServer'";
      }
    },
  },
};
