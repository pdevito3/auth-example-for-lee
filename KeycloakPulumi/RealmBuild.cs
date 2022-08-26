namespace KeycloakPulumi;

using KeycloakPulumi.Extensions;
using KeycloakPulumi.Factories;
using Pulumi;
using Pulumi.Keycloak;
using Pulumi.Keycloak.Inputs;

class RealmBuild : Stack
{
    public RealmBuild()
    {
        var realm = new Realm("DevRealm-realm", new RealmArgs
        {
            RealmName = "DevRealm",
            RegistrationAllowed = true,
            ResetPasswordAllowed = true,
            RememberMe = true,
            EditUsernameAllowed = true
        });
        var recipemanagementScope = ScopeFactory.CreateScope(realm.Id, "recipe_management");
        
        var recipeManagementPostmanMachineClient = ClientFactory.CreateClientCredentialsFlowClient(realm.Id,
            "recipe_management.postman.machine", 
            "974d6f71-d41b-4601-9a7a-a33081f84682", 
            "RecipeManagement Postman Machine",
            "https://oauth.pstmn.io");
        recipeManagementPostmanMachineClient.ExtendDefaultScopes(recipemanagementScope.Name);
        recipeManagementPostmanMachineClient.AddAudienceMapper("recipe_management");
        
        var recipeManagementPostmanCodeClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.postman.code", 
            "974d6f71-d41b-4601-9a7a-a33081f84680", 
            "RecipeManagement Postman Code",
            "https://oauth.pstmn.io",
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementPostmanCodeClient.ExtendDefaultScopes(recipemanagementScope.Name);
        recipeManagementPostmanCodeClient.AddAudienceMapper("recipe_management");
        
        var recipeManagementSwaggerClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.swagger", 
            "974d6f71-d41b-4601-9a7a-a33081f80687", 
            "RecipeManagement Swagger",
            "https://localhost:5375",
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementSwaggerClient.ExtendDefaultScopes(recipemanagementScope.Name);
        recipeManagementSwaggerClient.AddAudienceMapper("recipe_management");
        
        var recipeManagementBFFClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.bff", 
            "974d6f71-d41b-4601-9a7a-a33081f80688", 
            "RecipeManagement BFF",
            "https://localhost:4378",
            redirectUris: new InputList<string>() 
                {
                "https://localhost:4378/*",
                },
            webOrigins: new InputList<string>() 
                {
                "https://localhost:5375",
                "https://localhost:4378",
                }
            );
        recipeManagementBFFClient.ExtendDefaultScopes(recipemanagementScope.Name);
        recipeManagementBFFClient.AddAudienceMapper("recipe_management");
        
        var recipeManagementNextClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.next", 
            "974d6f71-d41b-4601-9a7a-a33081f82188", 
            "RecipeManagement Next",
            "http://localhost:8582",
            redirectUris: new InputList<string>() 
                {
                "http://localhost:8582/*",
                },
            webOrigins: new InputList<string>() 
                {
                "https://localhost:5375",
                "http://localhost:8582",
                }
            );
        recipeManagementNextClient.ExtendDefaultScopes(recipemanagementScope.Name);
        recipeManagementNextClient.AddAudienceMapper("recipe_management");
        
        var realmSuperAdmin = new Role("Super Admin", new RoleArgs
        {
            RealmId = realm.Id,
            Name = "Super Admin",
            Description = "Super Admin Role",
        });
        var realmUser = new Role("User", new RoleArgs
        {
            RealmId = realm.Id,
            Name = "User",
            Description = "User Role",
        });
        var defaultRoles = new DefaultRoles("default-roles", new DefaultRolesArgs()
        {
            RealmId = realm.Id,
            RoleNames = 
            {
                "uma_authorization",
                "offline_access",
                "User"
            },
        });

        var bob = new User("bob", new UserArgs
        {
            RealmId = realm.Id,
            Username = "bob",
            Enabled = true,
            Email = "bob@domain.com",
            FirstName = "Smith",
            LastName = "Bobson",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "bob",
                Temporary = true,
            },
        });

        var alice = new User("alice", new UserArgs
        {
            RealmId = realm.Id,
            Username = "alice",
            Enabled = true,
            Email = "alice@domain.com",
            FirstName = "Alice",
            LastName = "Smith",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "alice",
                Temporary = true,
            },
        });
        alice.SetRoles(realmSuperAdmin.Id);
    }
}