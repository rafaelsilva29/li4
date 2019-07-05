using CookieFy.Models;
using Microsoft.EntityFrameworkCore;

namespace CookieFy.Data
{
    public class CookieDbContext : DbContext
    {
        public CookieDbContext(DbContextOptions<CookieDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<RecipePlan> RecipesPlan { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<RecipeList> RecipeLists { get; set; }
        public DbSet<Step> Step { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Utensil> Utensil { get; set; }
        public DbSet<UserSubscription> UserSubscription { get; set; }
        public DbSet<RecipeUtensils> RecipeUtensils { get; set; }
        public DbSet<RecipeTag> RecipeTag { get; set; }
        public DbSet<RecipePlan> RecipePlan { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        public DbSet<RecipeList> RecipeList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -------------- USERS -------------- //

            // User's Subscriptions !! Many-to-Many
            modelBuilder.Entity<UserSubscription>()
                //.HasKey(c => new { c.UserID, c.SubscriptionID });
                .HasKey(us => new { us.UserSubscriptionID });
            modelBuilder.Entity<UserSubscription>()
                .HasOne( us => us.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(us => us.UserID);
            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.Subscription)
                .WithMany(s => s.UserSubscriptions)
                .HasForeignKey(us => us.SubscriptionID);
            // -----------------------------------------------

            // User's RecipeList !! One-to-Many
            modelBuilder.Entity<User>()
                .HasMany(u => u.RecipeLists)
                .WithOne(rl => rl.User);
            // -----------------------------------------------

            // User's Follows !! Auto-Relation
            modelBuilder.Entity<User>()
                .HasMany(u => u.Follows)
                .WithOne(uf => uf.UserFollow);
            // -----------------------------------------------

            // User's Recipes !! One-to-Many
            modelBuilder.Entity<User>()
                .HasMany(u => u.Recipes)
                .WithOne(r => r.User);
            // -----------------------------------------------

            // User's Comment !! Many-to-Many
            modelBuilder.Entity<Comment>()
                //.HasKey(c => new { c.UserID, c.RecipeID });
                .HasKey(c => new { c.CommentID });
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Recipe)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.RecipeID);
            // -----------------------------------------------
            // User's Recipes Plan !! Many-to-Many
            modelBuilder.Entity<RecipePlan>()
                //.HasKey(c => new { c.UserID, c.RecipeID });
                .HasKey(c => new { c.RecipePlanID });
            modelBuilder.Entity<RecipePlan>()
                .HasOne(c => c.User)
                .WithMany(u => u.RecipesPlan)
                .HasForeignKey(c => c.UserID);
            modelBuilder.Entity<RecipePlan>()
                .HasOne(c => c.Recipe)
                .WithMany(r => r.RecipesPlan)
                .HasForeignKey(c => c.RecipeID);
            // -----------------------------------------------

            // -------------- RECIPE -------------- //

            // Recipes RecipeUtensils !! Many-to-Many
            modelBuilder.Entity<RecipeUtensils>()
                //.HasKey(ru => new { ru.RecipeID, ru.UtensilID });
                .HasKey(c => new { c.RecipeUtensilsID });
            modelBuilder.Entity<RecipeUtensils>()
                .HasOne(ru => ru.Recipe)
                .WithMany(r => r.RecipeUtensils)
                .HasForeignKey(ru => ru.RecipeID);
            modelBuilder.Entity<RecipeUtensils>()
                .HasOne(ru => ru.Utensil)
                .WithMany(u => u.RecipeUtensils)
                .HasForeignKey(ru => ru.UtensilID);
            // -----------------------------------------------

            // Recipes RecipeIngredients!! Many-to-Many
            modelBuilder.Entity<RecipeIngredient>()
                //.HasKey(ri => new { ri.RecipeID, ri.IngredientID });
                .HasKey(c => new { c.RecipeIngredientID });
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeID);
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientID);
            // -----------------------------------------------

            // Recipes RecipeTags!! Many-to-Many
            modelBuilder.Entity<RecipeTag>()
                //.HasKey(rt => new { rt.RecipeID, rt.TagID });
                .HasKey(c => new { c.RecipeTagID });
            modelBuilder.Entity<RecipeTag>()
                .HasOne(rt => rt.Recipe)
                .WithMany(r => r.RecipeTags)
                .HasForeignKey(rt => rt.RecipeID);
            modelBuilder.Entity<RecipeTag>()
                .HasOne(rt => rt.Tag)
                .WithMany(t => t.RecipeTags)
                .HasForeignKey(rt => rt.TagID);
            // -----------------------------------------------

            // Recipes SubRecipes !! Auto-Relation
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.SubRecipes)
                .WithOne(r => r.SubRecipe).IsRequired(false);
            // -----------------------------------------------

            // Recipes Steps !! One-to-Many
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Steps)
                .WithOne(s => s.Recipe);
            // -----------------------------------------------

            // -----------------------------------------------
            // ---------------  Seeding data ----------------- 
            // -----------------------------------------------


            /*modelBuilder.Entity<User>()
                .HasData(
                    new User { UserID = 1, Name = "Rafael", Profession = "Estudante", Country = "Portugal", City = "Braga", Balance = 0, Email = "auqlads@gmail.com", ImgPath = "", Description = "", Password = "" },
                    new User { UserID = 2, Name = "Gonçalo", Profession = "Estudante", Country = "Portugal", City = "Braga", Balance = 0, Email = "auqlads@gmail.com", ImgPath = "", Description = "", Password = "" },
                    new User { UserID = 3, Name = "Rui", Profession = "Estudante", Country = "Portugal", City = "Braga", Balance = 0, Email = "auqlads@gmail.com", ImgPath = "", Description = "", Password = "" }
                );

            modelBuilder.Entity<RecipeType>()
                .HasData(
                    new RecipeType { RecipeTypeID = 1, Name = "Free", Price = 0 }
                );

            modelBuilder.Entity<Tag>()
                .HasData(
                    new Tag { TagID = 1, Name = "Beef" },
                    new Tag { TagID = 2, Name = "Rice" },
                    new Tag { TagID = 3, Name = "Fish" },
                    new Tag { TagID = 4, Name = "Gluten free" }
                );

            modelBuilder.Entity<Ingredient>()
                .HasData(
                    new Ingredient { IngredientID = 1, Name = "Sal" },
                    new Ingredient { IngredientID = 2, Name = "Açucar" },
                    new Ingredient { IngredientID = 3, Name = "Pimenta" },
                    new Ingredient { IngredientID = 4, Name = "Cogumelo" },
                    new Ingredient { IngredientID = 5, Name = "Azeitona" },
                    new Ingredient { IngredientID = 6, Name = "Mel" },
                    new Ingredient { IngredientID = 7, Name = "Arroz" }
                );

            modelBuilder.Entity<Utensil>()
                .HasData(
                    new Utensil { UtensilID = 1, Name = "Varinha Mágica" },
                    new Utensil { UtensilID = 2, Name = "Colher de Pau" },
                    new Utensil { UtensilID = 3, Name = "Batedeira" },
                    new Utensil { UtensilID = 4, Name = "Descascador de Batatas" },
                    new Utensil { UtensilID = 5, Name = "Picadora" }
                );

            modelBuilder.Entity<Subscription>()
                .HasData(
                    new Subscription { SubscriptionID = 1, Name = "Free", Price = 0 },
                    new Subscription { SubscriptionID = 2, Name = "Semanal", Price = 3.99 },
                    new Subscription { SubscriptionID = 3, Name = "Mensal", Price = 10.99 },
                    new Subscription { SubscriptionID = 4, Name = "Anual", Price = 99.99 }
                );

            modelBuilder.Entity<Recipe>()
                .HasData(
                    new Recipe { RecipeID = 1, Title = "Massa com atum", Description = "Marabilha marabilha.", Rank = 0, Classifications = 0, Time = 10, RecipeTypeID = 1, UserID = 1 }
                );

            modelBuilder.Entity<Step>()
                .HasData(
                    new Step { StepID = 1, RecipeID = 1, Position = 1, Text = "Panados com pão 1", Time = 10 },
                    new Step { StepID = 2, RecipeID = 1, Position = 2, Text = "Panados com pão 2", Time = 10 },
                    new Step { StepID = 3, RecipeID = 1, Position = 3, Text = "Panados com pão 3", Time = 10 },
                    new Step { StepID = 4, RecipeID = 1, Position = 4, Text = "Panados com pão 4", Time = 10 }
                );*/
        }
    }
}