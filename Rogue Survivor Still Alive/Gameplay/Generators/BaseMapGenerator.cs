using System;
using System.Drawing;

using djack.RogueSurvivor.Data;
using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Engine.Items;
using djack.RogueSurvivor.Engine.MapObjects;

namespace djack.RogueSurvivor.Gameplay.Generators
{
    abstract class BaseMapGenerator : MapGenerator
    {
        #region Fields
        protected readonly RogueGame m_Game;
        #endregion

        #region Init
        protected BaseMapGenerator(RogueGame game)
            : base(game.Rules)
        {
            m_Game = game;
        }
        #endregion

        #region Actor dressing helpers
        static readonly string[] MALE_SKINS = new string[] { GameImages.MALE_SKIN1, GameImages.MALE_SKIN2, GameImages.MALE_SKIN3, GameImages.MALE_SKIN4, GameImages.MALE_SKIN5 };
        static readonly string[] MALE_HEADS = new string[] { GameImages.MALE_HAIR1, GameImages.MALE_HAIR2, GameImages.MALE_HAIR3, GameImages.MALE_HAIR4, GameImages.MALE_HAIR5, GameImages.MALE_HAIR6, GameImages.MALE_HAIR7, GameImages.MALE_HAIR8 };
        static readonly string[] MALE_TORSOS = new string[] { GameImages.MALE_SHIRT1, GameImages.MALE_SHIRT2, GameImages.MALE_SHIRT3, GameImages.MALE_SHIRT4, GameImages.MALE_SHIRT5 };
        static readonly string[] MALE_LEGS = new string[] { GameImages.MALE_PANTS1, GameImages.MALE_PANTS2, GameImages.MALE_PANTS3, GameImages.MALE_PANTS4, GameImages.MALE_PANTS5 };
        static readonly string[] MALE_SHOES = new string[] { GameImages.MALE_SHOES1, GameImages.MALE_SHOES2, GameImages.MALE_SHOES3, GameImages.MALE_SHOES4, GameImages.MALE_SHOES5 };
        static readonly string[] MALE_EYES = new string[] { GameImages.MALE_EYES1, GameImages.MALE_EYES2, GameImages.MALE_EYES3, GameImages.MALE_EYES4, GameImages.MALE_EYES5, GameImages.MALE_EYES6 };

        static readonly string[] FEMALE_SKINS = new string[] { GameImages.FEMALE_SKIN1, GameImages.FEMALE_SKIN2, GameImages.FEMALE_SKIN3, GameImages.FEMALE_SKIN4, GameImages.FEMALE_SKIN5 };
        static readonly string[] FEMALE_HEADS = new string[] { GameImages.FEMALE_HAIR1, GameImages.FEMALE_HAIR2, GameImages.FEMALE_HAIR3, GameImages.FEMALE_HAIR4, GameImages.FEMALE_HAIR5, GameImages.FEMALE_HAIR6, GameImages.FEMALE_HAIR7, GameImages.FEMALE_HAIR8 };
        static readonly string[] FEMALE_TORSOS = new string[] { GameImages.FEMALE_SHIRT1, GameImages.FEMALE_SHIRT2, GameImages.FEMALE_SHIRT3, GameImages.FEMALE_SHIRT4, GameImages.FEMALE_SHIRT5 };
        static readonly string[] FEMALE_LEGS = new string[] { GameImages.FEMALE_PANTS1, GameImages.FEMALE_PANTS2, GameImages.FEMALE_PANTS3, GameImages.FEMALE_PANTS4, GameImages.FEMALE_PANTS5 };
        static readonly string[] FEMALE_SHOES = new string[] { GameImages.FEMALE_SHOES1, GameImages.FEMALE_SHOES2, GameImages.FEMALE_SHOES3, GameImages.FEMALE_SHOES4, GameImages.FEMALE_SHOES5 };
        static readonly string[] FEMALE_EYES = new string[] { GameImages.FEMALE_EYES1, GameImages.FEMALE_EYES2, GameImages.FEMALE_EYES3, GameImages.FEMALE_EYES4, GameImages.FEMALE_EYES5, GameImages.FEMALE_EYES6 };

        static readonly string[] BIKER_HEADS = new string[] { GameImages.BIKER_HAIR1, GameImages.BIKER_HAIR2, GameImages.BIKER_HAIR3 };
        static readonly string[] BIKER_LEGS = new string[] { GameImages.BIKER_PANTS };
        static readonly string[] BIKER_SHOES = new string[] { GameImages.BIKER_SHOES };

        static readonly string[] CHARGUARD_HEADS = new string[] { GameImages.CHARGUARD_HAIR };
        static readonly string[] CHARGUARD_LEGS = new string[] { GameImages.CHARGUARD_PANTS };

        static readonly string[] CHARSCIENTIST_HEAD = new string[] { GameImages.CHARSCIENTIST_HEAD }; //@@MP (Release 8-1)
        static readonly string[] CHARSCIENTIST_TORSO = new string[] { GameImages.CHARSCIENTIST_SHIRT }; //@@MP (Release 8-1)
        static readonly string[] CHARSCIENTIST_LEGS = new string[] { GameImages.CHARSCIENTIST_PANTS }; //@@MP (Release 8-1)

        static readonly string[] PRISONER_TORSO = new string[] { GameImages.PRISONER_UNIFORM }; //@@MP (Release 7-6)
        static readonly string[] PRISONER_LEGS = new string[] { GameImages.PRISONER_PANTS }; //@@MP (Release 7-6)
        static readonly string[] PRISONER_SHOES = new string[] { GameImages.PRISONER_SHOES }; //@@MP (Release 7-6)

        static readonly string[] DOG_SKINS = new string[] { GameImages.DOG_SKIN1_EAST, GameImages.DOG_SKIN2_EAST, GameImages.DOG_SKIN3_EAST, GameImages.DOG_SKIN1_WEST, GameImages.DOG_SKIN2_WEST, GameImages.DOG_SKIN3_WEST };

        public void DressCivilian(DiceRoller roller, Actor actor)
        {
            if (actor.Model.DollBody.IsMale)
                DressCivilian(roller, actor, MALE_EYES, MALE_SKINS, MALE_HEADS, MALE_TORSOS, MALE_LEGS, MALE_SHOES);
            else
                DressCivilian(roller, actor, FEMALE_EYES, FEMALE_SKINS, FEMALE_HEADS, FEMALE_TORSOS, FEMALE_LEGS, FEMALE_SHOES);
        }

        public static void SkinNakedHuman(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            if (actor.Model.DollBody.IsMale)
                SkinNakedHuman(roller, actor, MALE_EYES, MALE_SKINS, MALE_HEADS);
            else
                SkinNakedHuman(roller, actor, FEMALE_EYES, FEMALE_SKINS, FEMALE_HEADS);
        }

        public static void DressCivilian(DiceRoller roller, Actor actor, string[] eyes, string[] skins, string[] heads, string[] torsos, string[] legs, string[] shoes) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.EYES, eyes[roller.Roll(0, eyes.Length)]);
            actor.Doll.AddDecoration(DollPart.SKIN, skins[roller.Roll(0, skins.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, heads[roller.Roll(0, heads.Length)]);
            actor.Doll.AddDecoration(DollPart.TORSO, torsos[roller.Roll(0, torsos.Length)]);
            actor.Doll.AddDecoration(DollPart.LEGS, legs[roller.Roll(0, legs.Length)]);
            actor.Doll.AddDecoration(DollPart.FEET, shoes[roller.Roll(0, shoes.Length)]);
        }

        public static void SkinNakedHuman(DiceRoller roller, Actor actor, string[] eyes, string[] skins, string[] heads) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.EYES, eyes[roller.Roll(0, eyes.Length)]);
            actor.Doll.AddDecoration(DollPart.SKIN, skins[roller.Roll(0, skins.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, heads[roller.Roll(0, heads.Length)]);
        }

        public static void SkinDog(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.SKIN, DOG_SKINS[roller.Roll(0, DOG_SKINS.Length)]);
        }

        public static void DressArmy(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, GameImages.ARMY_HELMET);
            actor.Doll.AddDecoration(DollPart.TORSO, GameImages.ARMY_SHIRT);
            actor.Doll.AddDecoration(DollPart.LEGS, GameImages.ARMY_PANTS);
            actor.Doll.AddDecoration(DollPart.FEET, GameImages.ARMY_SHOES);
        }

        public static void DressPolice(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.EYES, MALE_EYES[roller.Roll(0, MALE_EYES.Length)]);
            actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, MALE_HEADS[roller.Roll(0, MALE_HEADS.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, GameImages.POLICE_HAT);
            actor.Doll.AddDecoration(DollPart.TORSO, GameImages.POLICE_UNIFORM);
            actor.Doll.AddDecoration(DollPart.LEGS, GameImages.POLICE_PANTS);
            actor.Doll.AddDecoration(DollPart.FEET, GameImages.POLICE_SHOES);
        }

        public static void DressPrisoner(DiceRoller roller, Actor actor) //@@MP - added (Release 7-6)
        {
            actor.Doll.RemoveAllDecorations();
            if (actor.Model.DollBody.IsMale)
            {
                actor.Doll.AddDecoration(DollPart.EYES, MALE_EYES[roller.Roll(0, MALE_EYES.Length)]);
                actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
                actor.Doll.AddDecoration(DollPart.HEAD, MALE_HEADS[roller.Roll(0, MALE_HEADS.Length)]);
            }
            else
            {
                actor.Doll.AddDecoration(DollPart.EYES, FEMALE_EYES[roller.Roll(0, FEMALE_EYES.Length)]);
                actor.Doll.AddDecoration(DollPart.SKIN, FEMALE_SKINS[roller.Roll(0, FEMALE_SKINS.Length)]);
                actor.Doll.AddDecoration(DollPart.HEAD, FEMALE_HEADS[roller.Roll(0, FEMALE_HEADS.Length)]);
            }
            actor.Doll.AddDecoration(DollPart.TORSO, GameImages.PRISONER_UNIFORM);
            actor.Doll.AddDecoration(DollPart.LEGS, GameImages.PRISONER_PANTS);
            actor.Doll.AddDecoration(DollPart.FEET, GameImages.PRISONER_SHOES);
        }

        public static void DressBiker(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.EYES, MALE_EYES[roller.Roll(0, MALE_EYES.Length)]);
            actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, BIKER_HEADS[roller.Roll(0, BIKER_HEADS.Length)]);
            actor.Doll.AddDecoration(DollPart.LEGS, BIKER_LEGS[roller.Roll(0, BIKER_LEGS.Length)]);
            actor.Doll.AddDecoration(DollPart.FEET, BIKER_SHOES[roller.Roll(0, BIKER_SHOES.Length)]);
        }

        public static void DressGangsta(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.EYES, MALE_EYES[roller.Roll(0, MALE_EYES.Length)]);
            actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
            actor.Doll.AddDecoration(DollPart.TORSO, GameImages.GANGSTA_SHIRT);
            actor.Doll.AddDecoration(DollPart.HEAD, MALE_HEADS[roller.Roll(0, MALE_HEADS.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, GameImages.GANGSTA_HAT);
            actor.Doll.AddDecoration(DollPart.LEGS, GameImages.GANGSTA_PANTS);
            actor.Doll.AddDecoration(DollPart.FEET, MALE_SHOES[roller.Roll(0, MALE_SHOES.Length)]);
        }
        
        public static void DressCHARGuard(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.EYES, MALE_EYES[roller.Roll(0, MALE_EYES.Length)]);
            actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
            actor.Doll.AddDecoration(DollPart.HEAD, CHARGUARD_HEADS[roller.Roll(0, CHARGUARD_HEADS.Length)]);
            actor.Doll.AddDecoration(DollPart.LEGS, CHARGUARD_LEGS[roller.Roll(0, CHARGUARD_LEGS.Length)]);
        }

        public static void DressCHARScientist(DiceRoller roller, Actor actor) //@@MP (Release 8-1)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
            actor.Doll.AddDecoration(DollPart.TORSO, GameImages.CHARSCIENTIST_SHIRT);
            actor.Doll.AddDecoration(DollPart.HEAD, GameImages.CHARSCIENTIST_HEAD);
            actor.Doll.AddDecoration(DollPart.LEGS, GameImages.CHARSCIENTIST_PANTS);
        }

        public static void DressBlackOps(DiceRoller roller, Actor actor) //@@MP - made static (Release 5-7)
        {
            actor.Doll.RemoveAllDecorations();
            actor.Doll.AddDecoration(DollPart.EYES, MALE_EYES[roller.Roll(0, MALE_EYES.Length)]);
            actor.Doll.AddDecoration(DollPart.SKIN, MALE_SKINS[roller.Roll(0, MALE_SKINS.Length)]);
            actor.Doll.AddDecoration(DollPart.TORSO, GameImages.BLACKOP_SUIT);
        }

        public static string RandomSkin(DiceRoller roller, bool isMale) //@@MP - made static (Release 5-7)
        {
            string[] skins = isMale ? MALE_SKINS : FEMALE_SKINS;
            return skins[roller.Roll(0, skins.Length)];
        }

        public static void RandomiseTorsoClothing(DiceRoller roller, Actor actor) //@@MP - added (Release 7-6)
        {
            actor.Doll.RemoveDecoration(DollPart.TORSO);
            if (actor.Doll.Body.IsMale) //not trying to be sexist, it's just that male and female base body sprites are shaped slightly differently and so the clothes must fit the sprite
                actor.Doll.AddDecoration(DollPart.TORSO, MALE_TORSOS[roller.Roll(0, MALE_TORSOS.Length)]);
            else
                actor.Doll.AddDecoration(DollPart.TORSO, FEMALE_TORSOS[roller.Roll(0, FEMALE_TORSOS.Length)]);
        }

        public static void RandomiseLegsClothing(DiceRoller roller, Actor actor) //@@MP - added (Release 7-6)
        {
            actor.Doll.RemoveDecoration(DollPart.LEGS);
            if (actor.Doll.Body.IsMale) //not trying to be sexist, it's just that male and female base body sprites are shaped slightly differently and so the clothes must fit the sprite
                actor.Doll.AddDecoration(DollPart.LEGS, MALE_LEGS[roller.Roll(0, MALE_LEGS.Length)]);
            else
                actor.Doll.AddDecoration(DollPart.LEGS, FEMALE_LEGS[roller.Roll(0, FEMALE_LEGS.Length)]);
        }

        public static void RandomiseShoes(DiceRoller roller, Actor actor)   //@@MP (Release 7-6)
        {
            actor.Doll.RemoveDecoration(DollPart.FEET);
            if (actor.Doll.Body.IsMale) //not trying to be sexist, it's just that male and female base body sprites are shaped slightly differently and so the shoes must fit the sprite
                actor.Doll.AddDecoration(DollPart.FEET, MALE_SHOES[roller.Roll(0, MALE_SHOES.Length)]);
            else
                actor.Doll.AddDecoration(DollPart.FEET, FEMALE_SHOES[roller.Roll(0, FEMALE_SHOES.Length)]);
        }

        public static void RandomiseHair(DiceRoller roller, Actor actor)   //@@MP (Release 7-6)
        {
            actor.Doll.RemoveDecoration(DollPart.HEAD);
            if (actor.Doll.Body.IsMale) //not trying to be sexist, it's just that male and female base body sprites are shaped slightly differently and so the hair must fit the sprite
                actor.Doll.AddDecoration(DollPart.HEAD, MALE_HEADS[roller.Roll(0, MALE_HEADS.Length)]);
            else
                actor.Doll.AddDecoration(DollPart.HEAD, FEMALE_HEADS[roller.Roll(0, FEMALE_HEADS.Length)]);
        }
        #endregion

        #region Actor naming helpers
        static readonly string[] MALE_FIRST_NAMES = 
        {
            "Aaron", "Adam", "Adrian", "Alan", "Albert", "Alberto", "Alex", "Alexander", "Alfred", "Alfredo", "Allan", "Allen", "Alvin", "Andre", "Andrew", "Andy", "Angel", "Anton", "Antonio", "Anthony", "Armando", "Arnold", "Arthur", "Ashley", "Axel",
            "Barry", "Ben", "Benjamin", "Bernard", "Bill", "Billy", "Bob", "Bobby", "Brad", "Brandon", "Bradley", "Brent", "Brett", "Brian", "Bryan", "Bruce", "Byron",
            "Caine", "Calvin", "Carl", "Carlos", "Carlton", "Casey", "Cecil", "Chad", "Charles", "Charlie", "Chester", "Chris", "Christian", "Christopher", "Clarence", "Clark", "Claude", "Clayton", "Clifford", "Clifton", "Clinton", "Clyde", "Cody", "Corey", "Cory", "Craig", "Cris", "Cristobal", "Curtis",
            "Dan", "Daniel", "Danny", "Dale", "Darrell", "Darren", "Darryl", "Dave", "David", "Dean", "Dennis", "Derek", "Derrick", "Dirk", "Don", "Donald", "Donovan", "Doug", "Douglas", "Duane", "Dustin", "Dwayne", "Dwight",
            "Earl", "Ed", "Eddie", "Eddy", "Edgar", "Eduardo", "Edward", "Edwin", "Elias", "Elie", "Elmer", "Elton", "Enrique", "Eric", "Erik", "Ernest", "Eugene", "Everett","Eli",
            "Felix", "Fernando", "Floyd", "Francis", "Francisco", "Frank", "Franklin", "Fred", "Frederick", "Freddie",
            "Gabriel", "Gary", "Gene", "George", "Georges", "Gerald", "Gilbert", "Glenn", "Gordon", "Greg", "Gregory", "Guy",
            "Hank", "Harold", "Harvey", "Harry", "Hector", "Henry", "Herbert", "Herman", "Howard", "Hubert", "Hugh", "Hughie",
            "Ian", "Indy", "Isaac", "Ivan",
            "Jack", "Jacob", "Jaime", "Jake", "James", "Jamie", "Jared", "Jarvis", "Jason", "Javier", "Jay", "Jeff", "Jeffrey", "Jeremy", "Jerome", "Jerry", "Jesse", "Jessie", "Jesus", "Jim", "Jimmie", "Jimmy", "Joe", "Joel", "John", "Johnnie", "Johnny", "Jon", "Jonas", "Jonathan", "Jordan", "Jorge", "Jose", "Joseph", "Joshua", "Juan", "Julian", "Julio", "Justin",
            "Kane","Karl", "Keith", "Kelly", "Ken", "Kenneth", "Kent", "Kevin", "Kirk", "Kurt", "Kyle",
            "Lance", "Larry", "Lars", "Lawrence", "Lee", "Lennie", "Leo", "Leon", "Leonard", "Leroy", "Leslie", "Lester", "Lewis", "Lloyd", "Lonnie", "Louis", "Luis",
            "Manuel", "Marc", "Marcus", "Mario", "Mark", "Marshall", "Martin", "Marvin", "Maurice", "Matthew", "Max", "Melvin", "Michael", "Mickey", "Miguel", "Mike", "Milton", "Mitch", "Mitchell", "Morris",
            "Nathan", "Nathaniel", "Ned", "Neil", "Nelson", "Nicholas", "Nick", "Norman",
            "Oliver", "Orlando", "Oscar",
            "Pablo", "Patrick", "Paul", "Pedro", "Perry", "Pete", "Peter", "Phil", "Phillip", "Preston",
            "Quentin",
            "Rafael", "Ralph", "Ramon", "Randall", "Randy", "Raul", "Ray", "Raymond", "Reginald", "Rene", "Ricardo", "Richard", "Rick", "Ricky", "Rob", "Robert", "Roberto", "Rodney", "Roger", "Roland", "Ron", "Ronald", "Ronnie", "Ross", "Roy", "Ruben", "Rudy", "Russell", "Ryan",
            "Salvador", "Sam", "Samuel", "Saul", "Scott", "Sean", "Seth", "Sergio", "Shane", "Shaun", "Shawn", "Sidney", "Stan", "Stanley", "Stephen", "Steve", "Steven", "Stuart",
            "Ted", "Terrance", "Terrence", "Terry", "Theodore", "Thomas", "Tim", "Timothy", "Toby", "Todd", "Tom", "Tommy", "Tony", "Tracy", "Travis", "Trevor", "Troy", "Tyler", "Tyrone",
            "Ulrich",
            "Val", "Vernon", "Vince", "Vincent", "Vinnie", "Victor", "Virgil",
            "Wade", "Wallace", "Walter", "Warren", "Wayne", "Wesley", "Willard", "William", "Willie",
            "Xavier","Xander",
            // Y
            "Zachary", "Zack"
        };

        static readonly string[] FEMALE_FIRST_NAMES = 
        {
            "Abigail", "Agnes", "Ali", "Alice", "Alicia", "Allison", "Alma", "Amanda", "Amber", "Amy", "Andrea", "Angela", "Anita", "Ana", "Ann", "Anna", "Anne", "Annette", "Annie", "April", "Arlene", "Ashley", "Audrey",
            "Barbara", "Beatrice", "Becky", "Belinda", "Bernice", "Bertha", "Bessie", "Beth", "Betty", "Beverly", "Billie", "Bobbie", "Bonnie", "Brandy", "Brenda", "Britanny",
            "Carla", "Carmen", "Carol", "Carole", "Caroline", "Carolyn", "Carrie", "Cassandra", "Cassie", "Cathy", "Catherine", "Charlene", "Charlotte", "Cherie", "Cheryl", "Christina", "Christine", "Christy", "Cindy", "Claire", "Clara", "Claudia", "Colleen", "Connie", "Constance", "Courtney", "Cris", "Crissie", "Crystal",  "Cynthia",
            "Daisy", "Dana", "Danielle", "Darlene", "Dawn", "Deanna", "Debbie", "Deborah", "Debrah", "Delores", "Denise", "Diana", "Diane", "Donna", "Dolores", "Dora", "Doris", "Dorothy",
            "Edith", "Edna", "Eileen", "Elaine", "Elayne", "Eleanor", "Eleonor", "Elizabeth", "Ella", "Ellen", "Elsie", "Emily", "Emma", "Erica", "Erika", "Erin", "Esther", "Ethel", "Eva", "Evelyn","Effie",
            "Felicia", "Fiona", "Florence", "Fran", "Frances","Felicity","Fifi",
            "Gail", "Georgia", "Geraldine", "Gertrude", "Gina", "Ginger", "Gladys", "Glenda", "Gloria", "Grace", "Gwendolyn",
            "Hazel", "Heather", "Heidi", "Helen", "Helena", "Hilary", "Hilda", "Holly","Hope",
            "Ida", "Ingrid", "Irene", "Irma", "Isabela",
            "Jackie", "Jacqueline", "Jamie", "Jane", "Janet", "Janice", "Jean", "Jeanne", "Jeanette", "Jennie", "Jennifer", "Jenny", "Jess", "Jessica", "Jessie", "Jill", "Jo", "Joan", "Joana", "Joanne", "Josephine", "Joy", "Joyce", "Juanita", "Judith", "Judy", "Julia", "Julie", "June",
            "Karen", "Kate", "Katherine", "Kathleen", "Kathy", "Kathryn", "Katie", "Katrina", "Kay", "Kelly", "Kim", "Kimberly", "Kira", "Kristen", "Kristin", "Kristina","Katya",
            "Laura", "Lauren", "Laurie", "Lea", "Lena", "Leona", "Leonor", "Leslie", "Lillian", "Lillie", "Linda", "Lindsay", "Lisa", "Liz", "Lois", "Loretta", "Lori", "Lorraine", "Louise", "Lucia", "Lucille", "Lucy", "Lydia", "Lynn",
            "Mabel", "Mae", "Maggie", "Marcia", "Margaret", "Margie", "Maria", "Marian", "Marie", "Marion", "Marjorie", "Marlene", "Marsha", "Martha", "Mary", "Marylin", "Mary-Ann", "Mattie", "Maureen", "Maxine", "Megan", "Melanie", "Melinda", "Melissa", "Michele", "Mildred", "Millie", "Minnie", "Miriam", "Misty", "Molly", "Monica", "Myrtle",
            "Naomi", "Nancy", "Natalie", "Nelly", "Nicole", "Nina", "Nora", "Norma",
            "Olga", "Ophelia",
            "Paquita", "Page", "Pamela", "Patricia", "Patsy", "Patty", "Paula", "Pauline", "Pearl", "Peggy", "Penny", "Phyllis", "Priscilla","Poppy",
            // Q
            "Rachel", "Ramona", "Raquel", "Rebecca", "Regina", "Renee", "Rhonda", "Rita", "Roberta", "Robin", "Rosa", "Rose", "Rosemary", "Ruby", "Ruth",
            "Sabrina", "Sally", "Samantha", "Sandra", "Sara", "Sarah", "Shannon", "Sharon", "Sheila", "Shelly", "Sherry", "Shirley", "Sofia", "Sonia", "Stacey", "Stacy", "Stella", "Stephanie", "Sue", "Susan", "Suzanne", "Sylvia",
            "Tabatha", "Tamara", "Tammy", "Tanya", "Tara", "Terri", "Terry", "Tess", "Thelma", "Theresa", "Tiffany", "Tina", "Toni", "Tonya", "Tori", "Tracey", "Tracy",
            // U
            "Valerie", "Vanessa", "Velma", "Vera", "Veronica", "Vickie", "Victoria", "Viola", "Violet", "Virginia", "Vivian",
            "Wanda", "Wendy", "Willie", "Wilma", "Winona",
            // X
            "Yolanda", "Yvone",
            "Zora"
        };

        static readonly string[] LAST_NAMES = 
        {
            "Adams", "Alexander", "Allen","Anderson", "Austin","Alsop","Andrews","Ablett","Ames","Ali","Atkin","Adkins","Ashton","Albrecht","Allard","Ashcroft","Abraham",
            "Bailey", "Baker", "Barnes", "Bell", "Bennett", "Black", "Bradley", "Brown", "Brooks", "Bryant", "Bush", "Butler", "Brewster","Boyer","Bowden","Bamford","Bird","Busby",
            "Campbell", "Carpenter", "Carter", "Clark", "Coleman", "Collins", "Cook", "Cooper", "Cordell", "Cox", "Chen","Carpenter","Camden",
            "Davis", "Diaz", "Dobbs","Davies","Dawson","Dixon","Durham","Dunkley","Dale","Downie","Dodson","Denton","Davidson","Dutton",
            "Edwards", "Engels", "Evans","Engels","Epstein","Ericson","Ellis","Everitt","Ellison","Erwin","Eckert","Eldridge","Everton",
            "Finch", "Flores", "Ford", "Forrester", "Foster", "Frampton","Farrow","Fitzpatrick","Fleming","Foreman","Flanagan","Fairfield","Forrest","Franklin",
            "Garcia", "Gates", "Gonzales", "Gonzalez", "Gray", "Green", "Griffin","Gavins","Gregson","Granger","Godfreys",
            "Hall", "Harris", "Hayes", "Henderson", "Hernandez", "Hewlett", "Hill", "Holtz", "Howard", "Hughes","Holtz","Himmel","Hampson","Harper",
            "Irvine","Ipswitch","Ivanovich","Irwin","Iles","Isaac","Ingram","Irving",
            "Jackson", "James", "Jenkins", "Johnson", "Jones","Jameson","Jefferson","Johnstone","Jacobs","Jarmel","Jarvis","Joseph","Jorgen",
            "Kelly", "Kennedy", "King","Kevins","Kellet", "Kessel","Kessler","Kimmel","Kwon",
            "Lambert", "Lesaint", "Lee", "Lewis", "Long", "Lopez","Lawrence","Levya","Lawler","Lewin",
            "Malory", "Martin", "Martinez", "McAllister", "McGready", "Miller", "Mitchell", "Moore", "Morgan", "Morris", "Murphy","McAllister","Maynard","McKay","Mackey",
            "Nelson", "Norton", "Newnes","Nguyen","Needham","Napier","Nixon","Nielsen","Ng","North","Nolan","Newton","Neal","Naylor","Norwood",
            "O'Brien", "Oswald", "Orson","Olsen","Ortega","Oneill","Ogden","Overton","Obrien","Olvera",
            "Parker", "Patterson", "Paul", "Perez", "Perry", "Peterson", "Phillips", "Powell", "Price", "Paulson", "Pitt","Peters","Partridge","Pagan","Park","Prentice","Pruitt",
            "Quinn","Quigley",
            "Ramirez", "Reed", "Reeves", "Richardson", "Rivera", "Roberts", "Robinson", "Rockwell", "Rodriguez", "Rogers", "Robertson", "Ross", "Russell", "Robertson","Reynolds",
            "Sanchez", "Sanders", "Scott", "Simmons", "Smith", "Stevens", "Stewart","Sanchez", "Steward","Sampson","Stone","Stuart",
            "Tarver", "Taylor", "Thomas", "Thompson", "Torres", "Turner", "Tulev","Truscott","Trafford","Turnbull","Trujillo","Tudor","Tambor",
            "Ulrich","Ulman","Ulfred","Upshaw","Unsworth","Underhill","Upton","Unger",
            "Vance","Vermont","Vincent","Vickers","Velez","Vogler","Vaughan",
            "Walker", "Ward", "Walters", "Washington", "Watson", "White", "Williams", "Wilson", "Wood", "Wright",
            "Xiao","Xavier",
            "Young","Yeager","Yanez","Younis","Yobst","York",
            "Zane","Zimmer","Zamora"
        };

        public void GiveNameToActor(DiceRoller roller, Actor actor)
        {
            if (actor.Model.DollBody.IsMale)
                GiveNameToActor(roller, actor, MALE_FIRST_NAMES, LAST_NAMES);
            else
                GiveNameToActor(roller, actor, FEMALE_FIRST_NAMES, LAST_NAMES);
        }

        public static void GiveNameToActor(DiceRoller roller, Actor actor, string[] firstNames, string[] lastNames)
        {
            actor.IsProperName = true;
            string randomName = firstNames[roller.Roll(0, firstNames.Length)] + " " + lastNames[roller.Roll(0, lastNames.Length)];
            actor.Name = randomName;
        }
        #endregion

        #region Actor skills helpers
        public void GiveRandomSkillsToActor(DiceRoller roller, Actor actor, int count)
        {
            for (int i = 0; i < count; i++)
                GiveRandomSkillToActor(roller, actor);
        }

        public void GiveRandomSkillToActor(DiceRoller roller, Actor actor)
        {
            Skills.IDs randomID;
            if (actor.Model.Abilities.IsUndead)
                randomID = Skills.RollUndead(roller);
            else
                randomID = Skills.RollLiving(roller);
            GiveStartingSkillToActor(actor, randomID);
        }

        public void GiveStartingSkillToActor(Actor actor, Skills.IDs skillID)
        {
            if (actor.Sheet.SkillTable.GetSkillLevel((int)skillID) >= Skills.MaxSkillLevel(skillID))
                return;

            actor.Sheet.SkillTable.AddOrIncreaseSkill((int)skillID);

            // recompute starting stats.
            RecomputeActorStartingStats(actor);
        }

        public void RecomputeActorStartingStats(Actor actor)
        {
            actor.HitPoints = m_Rules.ActorMaxHPs(actor);
            actor.StaminaPoints = m_Rules.ActorMaxSTA(actor);
            actor.FoodPoints = m_Rules.ActorMaxFood(actor);
            actor.SleepPoints = m_Rules.ActorMaxSleep(actor);
            actor.Sanity = m_Rules.ActorMaxSanity(actor);
            if (actor.Inventory != null)
                actor.Inventory.MaxCapacity = m_Rules.ActorMaxInv(actor);
        }
        #endregion

        #region Common map objects
        public DoorWindow MakeObjWoodenDoor()
        {
            return new DoorWindow("wooden door", GameImages.OBJ_WOODEN_DOOR_CLOSED, GameImages.OBJ_WOODEN_DOOR_OPEN, GameImages.OBJ_WOODEN_DOOR_BROKEN, DoorWindow.BASE_HITPOINTS, DoorWindow.STATE_CLOSED)
            {
                GivesWood = true
            };
        }

        protected static DoorWindow MakeObjHospitalDoor() //@@MP - made static (Release 5-7)
        {
            return new DoorWindow("door", GameImages.OBJ_HOSPITAL_DOOR_CLOSED, GameImages.OBJ_HOSPITAL_DOOR_OPEN, GameImages.OBJ_HOSPITAL_DOOR_BROKEN, DoorWindow.BASE_HITPOINTS, DoorWindow.STATE_CLOSED)
            {
                GivesWood = true
            };
        }

        protected static DoorWindow MakeObjCharDoor() //@@MP - made static (Release 5-7)
        {
            return new DoorWindow("CHAR door", GameImages.OBJ_CHAR_DOOR_CLOSED, GameImages.OBJ_CHAR_DOOR_OPEN, GameImages.OBJ_CHAR_DOOR_BROKEN, 4 * DoorWindow.BASE_HITPOINTS, DoorWindow.STATE_CLOSED)
            {
                GivesWood = true
            };
        }

        protected static DoorWindow MakeObjGlassDoor() //@@MP - made static (Release 5-7)
        {
            return new DoorWindow("glass door", GameImages.OBJ_GLASS_DOOR_CLOSED, GameImages.OBJ_GLASS_DOOR_OPEN, GameImages.OBJ_GLASS_DOOR_BROKEN, DoorWindow.BASE_HITPOINTS / 4, DoorWindow.STATE_CLOSED)
            {
                IsMaterialTransparent = true,
                BreaksWhenFiredThrough = true
            };
        }

        public DoorWindow MakeObjIronDoor(int state) //@@MP - made public (Release 6-3)
        {
            return new DoorWindow("iron door", GameImages.OBJ_IRON_DOOR_CLOSED, GameImages.OBJ_IRON_DOOR_OPEN, GameImages.OBJ_IRON_DOOR_BROKEN, 8 * DoorWindow.BASE_HITPOINTS, state)
            {
                IsAn = true,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static DoorWindow MakeObjRollerDoor() //@@MP (Release 4), made static (Release 5-7)
        {
            return new DoorWindow("roller door", GameImages.OBJ_ROLLER_DOOR_CLOSED, GameImages.OBJ_ROLLER_DOOR_OPEN, GameImages.OBJ_ROLLER_DOOR_BROKEN, 6 * DoorWindow.BASE_HITPOINTS, DoorWindow.STATE_CLOSED)
            {
                IsMetal = true, //@@MP (Release 5-4)
            };
        }

        protected static DoorWindow MakeObjWindow() //@@MP - made static (Release 5-7)
        {
            // windows as transparent doors.
            return new DoorWindow("window", GameImages.OBJ_WINDOW_CLOSED, GameImages.OBJ_WINDOW_OPEN, GameImages.OBJ_WINDOW_BROKEN, DoorWindow.BASE_HITPOINTS / 4, DoorWindow.STATE_CLOSED)
            {
                IsWindow = true,
                IsMaterialTransparent = true,
                //GivesWood = true, //@@MP (Release 5-3)
                BreaksWhenFiredThrough = true
            };
        }

        /// <summary>
        /// Chain wire and IS jumpable
        /// </summary>
        protected static MapObject MakeObjFence(string fenceImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("chain wire fence", fenceImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 10)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsMetal = true, //@@MP (Release 5-4)
                StandOnFovBonus = true
            };
        }

        /// <summary>
        /// Chain wire but is NOT jumpable
        /// </summary>
        protected static MapObject MakeObjKennelFence(string fenceImageID) //@@MP (Release 7-5)
        {
            return new MapObject("chain wire fence", fenceImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 10)
            {
                IsMaterialTransparent = true,
                IsMetal = true
            };
        }

        protected static MapObject MakeObjWoodenFence(string fenceImageID) //@@MP (Release 6-1)
        {
            return new MapObject("wooden fence", fenceImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true
            };
        }

        protected static MapObject MakeObjIronRailing(string fenceImageID) //@@MP (Release 7-6)
        {
            return new MapObject("iron railing", fenceImageID)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsAn = true,
                IsMetal = true
            };
        }

        protected static MapObject MakeObjIronFence(string fenceImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("iron fence", fenceImageID)
            {
                IsMaterialTransparent = true,
                IsAn = true,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static MapObject MakeObjIronGate(string gateImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("iron gate", gateImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 20) //@@MP - made unbreakable (Release 6-2)
            {
                IsMaterialTransparent = true,
                IsAn = true,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        public Fortification MakeObjSmallFortification(string imageID)
        {
            return new Fortification("small fortification", imageID, Fortification.SMALL_BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 4,
                JumpLevel = 1
            };
        }

        public Fortification MakeObjLargeFortification(string imageID)
        {
            return new Fortification("large fortification", imageID, Fortification.LARGE_BASE_HITPOINTS)
            {
                GivesWood = true
            };
        }

        protected static string[] PARK_TREES = { GameImages.OBJ_TREE1, GameImages.OBJ_TREE2, GameImages.OBJ_TREE3, GameImages.OBJ_TREE4 }; //@@MP (Release 7-3)

        public static MapObject MakeObjParkTree(DiceRoller roller) //@@MP (Release 7-3)
        {
            return MakeObjTree(PARK_TREES[roller.Roll(0, PARK_TREES.Length)]);
        }

        protected static MapObject MakeObjTree(string treeImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("tree", treeImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 20) //@@MP - made breakable (Release 7-6)
            {
                GivesWood = true
            };
        }

        public static MapObject MakeObjTreeStump(string treeImageID) //@@MP (Release 7-6)
        {
            return new MapObject("tree stump", treeImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 20)
            {
                IsMaterialTransparent = true,
                IsWalkable = true
            };
        }

        static string[] CARS = { GameImages.OBJ_CAR_BLUE_PHASE0, GameImages.OBJ_CAR_GREEN_PHASE0, GameImages.OBJ_CAR_RED_PHASE0, GameImages.OBJ_CAR_WHITE_PHASE0, GameImages.OBJ_POLICE_CAR_PHASE0 }; //@@MP - added police cars (Release 7-6)

        /// <summary>
        /// Makes a new wrecked car of a random model.
        /// </summary>
        protected static Car MakeObjWreckedCar(DiceRoller roller) //@@MP - made static (Release 5-7), made a Car object (Release 7-1)
        {
            return MakeObjCar(CARS[roller.Roll(0, CARS.Length)], "wrecked car", MapObject.Break.BROKEN, roller.Roll(0,30));  //@@MP - added random fuel level (Release 7-1)
        }

        /// <summary>
        /// Makes a new car : transparent, not walkable but jumpable, movable.
        /// </summary>
        protected static Car MakeObjCar(string carImageID, string name, MapObject.Break breakState, int fuelUnits) //@@MP - made static (Release 5-7), made a Car object (Release 7-1), made generic (not necessarily wrecked) (Release 7-3)
        {
            return new Car(name, carImageID, breakState, fuelUnits)  //@@MP - added random fuel level (Release 7-1), added breakstate (Release 7-3)
            {
                BreakState = breakState,
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsMovable = true,
                Weight = 100,
                StandOnFovBonus = true,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        /// <summary>
        /// Makes a new non-wrecked car of a random model for a dealership.
        /// </summary>
        protected static Car MakeObjDisplayCar(DiceRoller roller)
        {
            return MakeObjCar(CARS[roller.Roll(0, CARS.Length)], "display car", MapObject.Break.UNBREAKABLE, 0);
        }

        /// <summary>
        /// Makes a new non-wrecked car of a random model for a car park.
        /// </summary>
        protected static Car MakeObjAbandonedCar(DiceRoller roller)
        {
            return MakeObjCar(CARS[roller.Roll(0, CARS.Length)], "abandoned car", MapObject.Break.UNBREAKABLE, roller.Roll(30, 98));
        }

        protected static MapObject MakeObjShelf(string shelfImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("shelf", shelfImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsContainer = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 6
            };
        }

        protected static MapObject MakeObjClothesDisplay(string shelfImageID) //@@MP - added (Release 7-6)
        {
            return new MapObject("clothes display", shelfImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                GivesWood = true,
                IsMovable = true,
                Weight = 6,
                HoverDescription = "Bump into it to change your outfit."
            };
        }

        protected static MapObject MakeObjBench(string benchImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("bench", benchImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsCouch = true,
                IsMovable = true,
                GivesWood = true,
                Weight = 6
            };
        }

        protected static MapObject MakeObjIronBench(string benchImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("iron bench", benchImageID)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsCouch = true,
                IsAn = true,
                IsMovable = true,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static MapObject MakeObjBed(string bedImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("bed", bedImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsMaterialTransparent = true,
                IsCouch = true,
                JumpLevel = 1, //@@MP (Release 4)
                GivesWood = true,
                IsMovable = true,
                StandOnFovBonus = true,
                Weight = 6
            };
        }

        protected static MapObject MakeObjWardrobe(string wardrobeImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("wardrobe", wardrobeImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsContainer = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 10
            };
        }

        protected static MapObject MakeObjDrawer(string drawerImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("drawer", drawerImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsContainer = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 6
            };
        }

        protected static MapObject MakeObjTable(string tableImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("table", tableImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                IsMovable = true,
                Weight = 6
            };
        }

        protected static MapObject MakeObjChair(string chairImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("chair", chairImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS / 3)
            {
                IsMaterialTransparent = true,
                GivesWood = true,
                IsMovable = true,
                IsWalkable = true, //@@MP (Release 4)
                Weight = 1
            };
        }

        protected static MapObject MakeObjNightTable(string nightTableImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("night table", nightTableImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS / 3)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                IsMovable = true,
                Weight = 1
            };
        }

        protected static MapObject MakeObjFridge(string fridgeImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("fridge", fridgeImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 6) //@@MP - made unbreakable (Release 6-2)
            {
                IsContainer = true,
                IsMovable = true,
                Weight = 10,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static MapObject MakeObjJunk(string junkImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("junk", junkImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 3)
            {
                IsPlural = true,
                IsMaterialTransparent = true,
                JumpLevel = 1, //@@MP (Release 7-6)
                StandOnFovBonus = true,
                IsMovable = true,
                IsContainer = true, //@@MP (Release 5-3)
                GivesWood = true,
                Weight = 15
            };
        }

        protected static MapObject MakeObjBarrels(string barrelsImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("barrels", barrelsImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 2) //@@MP - made unbreakable (Release 6-2)
            {
                IsPlural = true,
                IsMaterialTransparent = true,
                IsMovable = false, //@@MP (Release 7-6)
                IsContainer = true, //@@MP (Release 5-3)
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        /// <summary>
        /// If you want to spawn one of these as already on fire, use RogueGame.ApplyOnFire() rather than set it yourself
        /// </summary>
        protected static Barrel MakeObjFireBarrel(string barrelImageID) //@@MP - added (Release 7-6)
        {
            return new Barrel("receptacle", barrelImageID, MapObject.Break.UNBREAKABLE, 0)
            {
                IsMaterialTransparent = true,
                IsContainer = true, //in case items where left there when the barrel was unlit
                IsMovable = true,
                IsWalkable = true,
                Weight = 4,
                FireState = MapObject.Fire.BURNABLE,
                IsMetal = true,
                HoverDescription = "Use matches to start the fire. Bump to add more wood once alight."
            };
        }

        /// <summary>
        /// If you want to spawn one of these as already on fire, use RogueGame.ApplyOnFire() rather than set it yourself
        /// </summary>
        public static Campfire MakeObjCampfire(string campfireImageID) //@@MP - added (Release 7-6)
        {
            return new Campfire("campfire", campfireImageID, MapObject.Break.BREAKABLE, 0)
            {
                IsMaterialTransparent = true,
                IsContainer = true, //in case items where left there when the fire was unlit
                IsMovable = false,
                IsWalkable = true,
                FireState = MapObject.Fire.BURNABLE,
                HoverDescription = "Use matches to start the fire. Bump to add more wood once alight."
            };
        }

        protected static PowerGenerator MakeObjPowerGenerator(string offImageID, string onImageID) //@@MP - made static (Release 5-7)
        {
            return new PowerGenerator("power generator", offImageID, onImageID)
            {
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        public MapObject MakeObjBoard(string imageID, string[] text)
        {
            return new Board("board", imageID, text);
        }

        //@@MP (Release 3)
        protected static MapObject MakeObjCHARvat(string vatImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("CHAR vat", vatImageID)
            {
                IsMaterialTransparent = true //@@MP (Release 6-5)
            };
        }

        protected static MapObject MakeObjWorkstation(string CHARdesktopImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("workstation", CHARdesktopImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                IsMovable = true,
                Weight = 10
            };
        }

        protected static MapObject MakeObjHouseDrawers(string houseDrawersImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("drawers", houseDrawersImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsContainer = true,
                IsPlural = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 10
            };
        }

        protected static MapObject MakeObjHouseShelves(string houseShelvesImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("shelves", houseShelvesImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsContainer = true,
                IsPlural = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 10
            };
        }

        protected static MapObject MakeObjPiano(string pianoImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("piano", pianoImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                //IsContainer = true, //@@MP (Release 5-3)
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                IsMovable = true,
                Weight = 15
            };
        }

        protected static MapObject MakeObjPottedPlant(string pottedPlantImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("potted plant", pottedPlantImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS / 6)
            {
                IsMaterialTransparent = true,
                IsMovable = true,
                IsWalkable = true, //@@MP (Release 4)
                Weight = 1
            };
        }

        protected static MapObject MakeObjTelevision(string televisionImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("television", televisionImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS / 3)
            {
                IsMaterialTransparent = true,
                IsMovable = true,
                Weight = 3,
                JumpLevel = 1,  //@@MP (Release 7-6)
                GivesWood = true  //@@MP (Release 5-3)
            };
        }

        protected static MapObject MakeObjStandingLamp(string standingLampImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("standing lamp", standingLampImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS / 6)
            {
                IsMaterialTransparent = true,
                IsMovable = true,
                IsWalkable = true,
                Weight = 1
            };
        }

        protected static MapObject MakeObjStoveOven(string stoveOvenImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("stove oven", stoveOvenImageID)
            {
                IsMaterialTransparent = true,
                IsContainer = true, //@@MP (Release 5-3)
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static MapObject MakeObjKitchenSink(string kitchenSinkImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("sink", kitchenSinkImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 4)
            {
                IsMaterialTransparent = true,
                IsContainer = true, //@@MP (Release 5-3)
                JumpLevel = 1,
                GivesWood = true
            };
        }

        protected static MapObject MakeObjBookshelves(string bookshelvesImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("bookshelves", bookshelvesImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsContainer = true,
                IsPlural = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 10
            };
        }

        protected static MapObject MakeObjCouch(string couchImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("couch", couchImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 4)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                IsMovable = true,
                IsCouch = true, //@@MP (Release 6-6)
                Weight = 3
            };
        }

        protected static MapObject MakeObjKitchenCounter(string kitchenCounterImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("counter", kitchenCounterImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 4)
            {
                IsContainer = true,
                JumpLevel = 1,
                IsMaterialTransparent = true,
                GivesWood = true
            };
        }

        /// <summary>
        /// Generic counter useful for bars, shops, etc
        /// </summary>
        /// <param name="barCounterImageID"></param>
        /// <returns></returns>
        protected static MapObject MakeObjCounter(string counterImageID) //@@MP (Release 5-3), made static (Release 5-7)
        {
            return new MapObject("counter", counterImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 4)
            {
                JumpLevel = 1,
                IsMaterialTransparent = true,
                GivesWood = true,
                StandOnFovBonus = true
            };
        }

        protected static MapObject MakeObjCheckout(string checkoutImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("checkout", checkoutImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 4)
            {
                IsMaterialTransparent = true,
                IsContainer = true, //@@MP (Release 5-3)
                //JumpLevel = 1, //@@MP (Release 5-3)
                GivesWood = true
            };
        }

        //@@MP (Release 4)
        public static MapObject MakeObjTombstone(string tombstoneImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("tombstone", tombstoneImageID)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                StandOnFovBonus = true
            };
        }

        protected static MapObject MakeObjWorkbench(string workbenchImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("workbench", workbenchImageID)
            {
                IsContainer = true, //@@MP (Release 5-3)
            };
        }

        public MapObject MakeObjOpenBankSafe(string banksafeImageID) //@@MP (Release 6-5)
        {
            return new MapObject("open bank safe", banksafeImageID)
            {
                IsMetal = true,
                IsWalkable = true,
                IsMovable = false,
                StandOnFovBonus = false,
                HoverDescription = "You can securely hide your items from NPCs here." //@@MP (Release 7-6)
            };
        }

        protected static MapObject MakeObjClosedBankSafe(string banksafeImageID) //@@MP (Release 6-5)
        {
            return new MapObject("closed bank safe", banksafeImageID)
            {
                IsMetal = true,
                IsMovable = false
            };
        }

        public MapObject MakeObjOwnedBankSafe(string banksafeImageID) //@@MP (Release 6-5)
        {
            return new MapObject("your secure bank safe", banksafeImageID)
            {
                IsMetal = true,
                IsWalkable = true,
                IsMovable = false,
                StandOnFovBonus = false,
                HoverDescription = "You're securely hiding your items from NPCs here." //@@MP (Release 7-6)
            };
        }

        protected static MapObject MakeObjBankTeller(string banktellerImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("bank teller", banktellerImageID)
            {
                IsMaterialTransparent = true, //@@MP (Release 5-3)
            };
        }

        protected static MapObject MakeObjReceptionDesk(string receptiondeskImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("reception desk", receptiondeskImageID)
            {
                IsContainer = true, //@@MP (Release 5-3)
                JumpLevel = 1,
                IsMaterialTransparent = true,
                StandOnFovBonus = true
            };
        }

        protected static MapObject MakeObjMachinery(string machineryImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("machinery", machineryImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsMaterialTransparent = true,
                IsContainer = true //@@MP (Release 5-3)
            };
        }

        protected static MapObject MakeObjCurtain(string curtainImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("curtain", curtainImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS / 6)
            {
                IsWalkable = true,
                IsMovable = true,
                Weight = 1
            };
        }

        //@@MP (Release 6-3)
        protected static MapObject MakeObjArmyRadioCupboard(string armyRadioCupboardImageID)
        {
            return new MapObject("radio equipment", armyRadioCupboardImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsContainer = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 15
            };
        }

        protected static MapObject MakeObjArmyFootlocker(string armyFootlockerImageID)
        {
            return new MapObject("footlocker", armyFootlockerImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                IsContainer = true,
                IsMovable = true,
                JumpLevel = 1,
                Weight = 5
            };
        }

        /// <summary>
        /// A 96x32 pixel image, dissected into 3 pieces and then arranged as:  1-2-3
        /// </summary>
        /// <param name="heliImageID"></param>
        /// <returns></returns>
        public MapObject MakeObjHelicopter(string heliImageID) //@@MP (Release 6-4), made smaller (Release 7-3)
        {
            return new MapObject("helicopter", heliImageID)
            {
                IsMetal = true
            };
        }

        protected static MapObject MakeObjFuelPump(string fuelPumpImageID) //@@MP (Release 7-1)
        {
            return new MapObject("fuel pump", fuelPumpImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 20)
            {
                IsMetal = true
            };
        }

        public MapObject MakeObjFuelPumpBroken(string fuelPumpBrokenImageID) //@@MP (Release 7-3)
        {
            return new MapObject("exploded fuel pump", fuelPumpBrokenImageID)
            {
                IsMetal = true
            };
        }

        //@@MP (Release 7-2)
        public MapObject MakeObjSmokeScreen(string smokeScreenImageID)
        {
            return new MapObject("smoke cloud", smokeScreenImageID)
            {
                IsMaterialTransparent = false,
                IsWalkable = true,
                IsMovable = false
            };
        }

        //@@MP (Release 7-3)
        protected static MapObject MakeObjFireTruck(string fireTruckImageID)
        {
            //For EW a 32x64 pixel image, dissected into 2 pieces and then arranged as:
            //  1-2
            //For NS a 64x32 pixel image, dissected into 2 pieces and then arranged as:
            //  1
            //  2
            return new MapObject("fire truck", fireTruckImageID)
            {
                JumpLevel = 1,
                StandOnFovBonus = true,
                IsMetal = true,
                IsMovable = false
            };
        }

        /// <summary>
        /// Generic plant to be used as a container for fruit or veggies
        /// </summary>
        /// <param name="name">name of the plant</param>
        /// <param name="plantImageID">can be any plant image</param>
        /// <returns></returns>
        protected static MapObject MakeObjFarmPlant(string name, string plantImageID)
        {
            return new MapObject(name, plantImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS / 20)
            {
                IsMaterialTransparent = true,
                IsContainer = true,
                IsWalkable = true
            };
        }

        protected static MapObject MakeObjTractor(string tractorImageID)
        {
            return new MapObject("tractor", tractorImageID)
            {
                JumpLevel = 1,
                StandOnFovBonus = true,
                IsMetal = true,
                IsMovable = false
            };
        }

        protected static DoorWindow MakeObjChainFenceGate(int state)
        {
            return new DoorWindow("chainlink gate", GameImages.OBJ_CHAINWIRE_GATE_CLOSED, GameImages.OBJ_CHAINWIRE_GATE_OPEN, GameImages.OBJ_CHAINWIRE_GATE_BROKEN, 2 * DoorWindow.BASE_HITPOINTS, state)
            {
                IsMaterialTransparent = true,
                IsMetal = true
            };
        }

        protected static MapObject MakeObjBasketballRing(string ringImageID)
        {
            return new MapObject("basketball ring", ringImageID)
            {
                IsMaterialTransparent = true,
                IsWalkable = true,
                IsMetal = true
            };
        }

        protected static Car MakeObjVan(string vanImageID, int fuelUnits)
        {
            return new Car("van", vanImageID, MapObject.Break.BROKEN, fuelUnits)
            {
                BreakState = MapObject.Break.BROKEN,
                JumpLevel = 1,
                IsMovable = true,
                Weight = 300,
                StandOnFovBonus = true,
                IsMetal = true
            };
        }

        protected static MapObject MakeObjBathroomBasin(string basinImageID)
        {
            return new MapObject("basin", basinImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsMaterialTransparent = true,
                IsContainer = true
            };
        }

        protected static MapObject MakeObjToilet(string toiletImageID)
        {
            return new MapObject("toilet", toiletImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                StandOnFovBonus = true
            };
        }

        protected static MapObject MakeObjHouseholdMachine(string machineImageID, string name)
        {
            return new MapObject(name, machineImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 6)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                StandOnFovBonus = true,
                IsMetal = true
            };
        }

        protected static MapObject MakeObjSeat(string seatImageID)
        {
            return new MapObject("seat", seatImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsMaterialTransparent = true,
                GivesWood = true,
                StandOnFovBonus = true,
                JumpLevel = 1,
            };
        }

        protected static MapObject MakeObjCinemaScreen(string screenImageID)
        {
            return new MapObject("screen", screenImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS);
        }

        //@@MP (Release 7-6)
        protected static MapObject MakeObjDisplayCase(string caseImageID)
        {
            return new MapObject("display case", caseImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsContainer = true,
                GivesWood = true,
                IsMovable = true,
                IsMaterialTransparent = true,
                BreaksWhenFiredThrough = true,
                Weight = 10
            };
        }

        public MapObject MakeObjBurialCross(string crossImageID, string hoverDescription)
        {
            return new MapObject("burial cross", crossImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS)
            {
                JumpLevel = 1,
                IsMaterialTransparent = true,
                HoverDescription = hoverDescription
            };
        }

        protected static MapObject MakeObjWigsDisplay(string tableImageID)
        {
            return new MapObject("wigs display", tableImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                GivesWood = true,
                IsMovable = true,
                Weight = 2,
                HoverDescription = "Bump into it to change your hairstyle."
            };
        }

        protected static MapObject MakeObjCHARtrolley(string trolleyImageID)
        {
            return new MapObject("CHAR trolley", trolleyImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsMetal = true,
                IsMovable = true
            };
        }

        #endregion

        #region Common tile decorations
        public static void DecorateOutsideWalls(Map map, Rectangle rect, Func<int, int, string> decoFn) //@@MP - made static (Release 5-7)
        {
            for (int x = rect.Left; x < rect.Right; x++)
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    Tile tile = map.GetTileAt(x, y);
                    if (tile.Model.IsWalkable)
                        continue;
                    if (tile.IsInside)
                        continue;

                    string deco = decoFn(x, y);
                    if (deco != null)
                        tile.AddDecoration(deco);
                }
        }
        #endregion

        #region Common items
        public Item MakeItemSmallMedikit()
        {
            return new ItemMedicine(m_Game.GameItems.SMALL_MEDIKIT)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.SMALL_MEDIKIT.StackingLimit)
            };
        }

        public Item MakeItemLargeMedikit()
        {
            return new ItemMedicine(m_Game.GameItems.LARGE_MEDIKIT);
        }

        public Item MakeItemPillsSTA()
        {
            return new ItemMedicine(m_Game.GameItems.PILLS_STA)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.PILLS_STA.StackingLimit)
            };
        }

        public Item MakeItemPillsSLP()
        {
            return new ItemMedicine(m_Game.GameItems.PILLS_SLP)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.PILLS_SLP.StackingLimit)
            };
        }

        public Item MakeItemPillsSAN()
        {
            return new ItemMedicine(m_Game.GameItems.PILLS_SAN)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.PILLS_SAN.StackingLimit)
            };
        }

        public Item MakeItemPillsAntiviral()
        {
            return new ItemMedicine(m_Game.GameItems.PILLS_ANTIVIRAL)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.PILLS_ANTIVIRAL.StackingLimit)
            };
        }

        public Item MakeItemGroceries()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;

            /*
            int max = WorldTime.TURNS_PER_DAY * m_Game.GameItems.RAW_FISH.BestBeforeDays;
            int min = max / 2;
            int freshUntil = timeNow + m_Rules.Roll(min, max);
            */

            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.GROCERIES.BestBeforeDays); //@@MP - removed randomised freshUntil, because who cares (Release 7-6)

            return new ItemFood(m_Game.GameItems.GROCERIES, freshUntil, false, false);
        }

        public Item MakeItemCannedFood()
        {
            // canned food not perishable.
            return new ItemFood(m_Game.GameItems.CANNED_FOOD)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.CANNED_FOOD.StackingLimit)
            };
        }

        public Item MakeItemCrowbar()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.CROWBAR)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.CROWBAR.StackingLimit)
            };
        }

        public Item MakeItemBaseballBat()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.BASEBALLBAT);
        }

        public Item MakeItemCombatKnife()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.COMBAT_KNIFE);
        }

        public Item MakeItemTruncheon()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.TRUNCHEON);
        }

        public Item MakeItemGolfClub()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.GOLFCLUB);
        }

        public Item MakeItemIronGolfClub()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.IRON_GOLFCLUB);
        }

        public Item MakeItemHugeHammer()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.HUGE_HAMMER);
        }

        public Item MakeItemSmallHammer()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SMALL_HAMMER);
        }

        public Item MakeItemShovel()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SHOVEL);
        }

        public Item MakeItemShortShovel()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SHORT_SHOVEL);
        }

        public ItemBarricadeMaterial MakeItemWoodenPlank()
        {
            return new ItemBarricadeMaterial(m_Game.GameItems.WOODENPLANK);
        }

        public Item MakeItemHuntingCrossbow()
        {
            return new ItemRangedWeapon(m_Game.GameItems.HUNTING_CROSSBOW);
        }

        public Item MakeItemBoltsAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_BOLTS);
        }

        public Item MakeItemHuntingRifle()
        {
            return new ItemRangedWeapon(m_Game.GameItems.HUNTING_RIFLE);
        }

        public Item MakeItemLightRifleAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_LIGHT_RIFLE);
        }

        public Item MakeItemPistol()
        {
            return new ItemRangedWeapon(m_Game.GameItems.PISTOL);
        }

        public Item MakeItemRevolver()
        {
            return new ItemRangedWeapon(m_Game.GameItems.REVOLVER);
        }

        public Item MakeItemRandomPistol()
        {
            int randomItem = m_Game.Rules.Roll(0, 3);
            switch (randomItem)
            {
                case 0: return new ItemRangedWeapon(m_Game.GameItems.PISTOL);
                case 1: return new ItemRangedWeapon(m_Game.GameItems.REVOLVER);
                case 2: return new ItemRangedWeapon(m_Game.GameItems.VINTAGE_PISTOL);
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeItemLightPistolAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_LIGHT_PISTOL);
        }

        public Item MakeItemShotgun()
        {
            return new ItemRangedWeapon(m_Game.GameItems.SHOTGUN);
        }

        public Item MakeItemShotgunAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_SHOTGUN);
        }

        public Item MakeItemCHARLightBodyArmor()
        {
            return new ItemBodyArmor(m_Game.GameItems.CHAR_LT_BODYARMOR);
        }

        public Item MakeItemBikerGangJacket(GameGangs.IDs gangId)
        {
            switch (gangId)
            {
                case GameGangs.IDs.BIKER_FREE_ANGELS:
                    return new ItemBodyArmor(m_Game.GameItems.FREE_ANGELS_JACKET);
                case GameGangs.IDs.BIKER_HELLS_SOULS:
                    return new ItemBodyArmor(m_Game.GameItems.HELLS_SOULS_JACKET);
                default:
                    throw new ArgumentException("unhandled biker gang");
            }
        }

        public Item MakeItemPoliceJacket()
        {
            return new ItemBodyArmor(m_Game.GameItems.POLICE_JACKET);
        }

        public Item MakeItemPoliceRiotArmor()
        {
            return new ItemBodyArmor(m_Game.GameItems.POLICE_RIOT);
        }

        public Item MakeItemHunterVest()
        {
            return new ItemBodyArmor(m_Game.GameItems.HUNTER_VEST);
        }

        public Item MakeItemCellPhone()
        {
            return new ItemTracker(m_Game.GameItems.CELL_PHONE);
        }

        public Item MakeItemSprayPaint()
        {
            // random color.
            ItemSprayPaintModel paintModel;
            int roll = m_Game.Rules.Roll(0, 4);
            switch (roll)
            {
                case 0: paintModel = m_Game.GameItems.SPRAY_PAINT1; break;
                case 1: paintModel = m_Game.GameItems.SPRAY_PAINT2; break;
                case 2: paintModel = m_Game.GameItems.SPRAY_PAINT3; break;
                case 3: paintModel = m_Game.GameItems.SPRAY_PAINT4; break;
                default:
                    throw new InvalidOperationException("unhandled roll");
            }

            return new ItemSprayPaint(paintModel)
            {
                IsForbiddenToAI = true //@@MP - no point in them having these (Release 7-6)
            };
        }

        public Item MakeItemPaintThinner()  //@@MP (Release 7-6)
        {
            return new ItemSprayPaint(m_Game.GameItems.PAINT_THINNER)
            {
                IsForbiddenToAI = true //@@MP - no point in them having these (Release 7-6)
            };
        }

        public Item MakeItemStenchKiller()
        {
            return new ItemSprayScent(m_Game.GameItems.STENCH_KILLER);
        }

        public Item MakeItemPrecisionRifle()
        {
            return new ItemRangedWeapon(m_Game.GameItems.PRECISION_RIFLE);
        }

        public Item MakeItemPrecisionRifleAmmo() //@@MP (Release 6-6)
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_PRECISION_RIFLE);
        }

        public Item MakeItemArmyRifle() //@@MP - added 3 new types (Release 7-6)
        {
            // random model.
            int roll = m_Game.Rules.Roll(0, 4);
            switch (roll)
            {
                case 0: return new ItemRangedWeapon(m_Game.GameItems.ARMY_RIFLE1);
                case 1: return new ItemRangedWeapon(m_Game.GameItems.ARMY_RIFLE2);
                case 2: return new ItemRangedWeapon(m_Game.GameItems.ARMY_RIFLE3);
                case 3: return new ItemRangedWeapon(m_Game.GameItems.ARMY_RIFLE4);
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeItemHeavyRifleAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_HEAVY_RIFLE);
        }

        public Item MakeItemArmyPistol()
        {
            return new ItemRangedWeapon(m_Game.GameItems.ARMY_PISTOL);
        }

        public Item MakeItemHeavyPistolAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_HEAVY_PISTOL);
        }

        public Item MakeItemArmyBodyArmor()
        {
            return new ItemBodyArmor(m_Game.GameItems.ARMY_BODYARMOR);
        }

        public Item MakeItemArmyRation()
        {
            /*// army rations fresh for 5 days.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + WorldTime.TURNS_PER_DAY * m_Game.GameItems.ARMY_RATION.BestBeforeDays;*/

            return new ItemFood(m_Game.GameItems.ARMY_RATION);//, freshUntil); @@MP - made them long-life (Release 4)
        }

        public Item MakeItemFlashlight()
        {
            return new ItemLight(m_Game.GameItems.FLASHLIGHT);
        }

        public Item MakeItemBigFlashlight()
        {
            return new ItemLight(m_Game.GameItems.BIG_FLASHLIGHT);
        }

        public Item MakeItemRandomFlashlight() //@@MP (Release 7-6)
        {
            return new ItemLight(m_Rules.RollChance(50) ? m_Game.GameItems.BIG_FLASHLIGHT : m_Game.GameItems.FLASHLIGHT);
        }

        public Item MakeItemZTracker()
        {
            return new ItemTracker(m_Game.GameItems.ZTRACKER);
        }

        public Item MakeItemBlackOpsGPS()
        {
            return new ItemTracker(m_Game.GameItems.BLACKOPS_GPS);
        }

        public Item MakeItemPoliceRadio()
        {
            return new ItemTracker(m_Game.GameItems.POLICE_RADIO);
        }

        public Item MakeItemGrenade()
        {
            return new ItemGrenade(m_Game.GameItems.GRENADE, m_Game.GameItems.GRENADE_PRIMED)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.GRENADE.StackingLimit)
            };
        }

        public Item MakeItemBearTrap()
        {
            return new ItemTrap(m_Game.GameItems.BEAR_TRAP);
        }

        public Item MakeItemSpikes()
        {
            return new ItemTrap(m_Game.GameItems.SPIKES)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.BARBED_WIRE.StackingLimit)
            };
        }

        public Item MakeItemBarbedWire()
        {
            return new ItemTrap(m_Game.GameItems.BARBED_WIRE)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.BARBED_WIRE.StackingLimit)
            };
        }

        public Item MakeItemCHARBook() //@@MP (Release 7-6)
        {
            return new ItemEntertainment(m_Game.GameItems.BOOK_CHAR);
        }

        public Item MakeItemBook(DiceRoller roller)
        {
            ItemModel[] NOVELS = { m_Game.GameItems.BOOK_BLUE, m_Game.GameItems.BOOK_GREEN, m_Game.GameItems.BOOK_RED }; //@@MP - added more sprites for books (Release 7-6)

            return new ItemEntertainment(NOVELS[roller.Roll(0, NOVELS.Length)]);
        }

        public Item MakeItemMagazines(DiceRoller roller)
        {
            ItemModel[] COVERS = { m_Game.GameItems.MAGAZINE1, m_Game.GameItems.MAGAZINE2, m_Game.GameItems.MAGAZINE3, m_Game.GameItems.MAGAZINE4 }; //@@MP - added more sprites for mags (Release 7-6)
            ItemModel model = (COVERS[roller.Roll(0, COVERS.Length)]);

            return new ItemEntertainment(model)
            {
                Quantity = m_Rules.Roll(1, model.StackingLimit)
            };
        }

        //@@MP (Release 3)
        public Item MakeItemTennisRacket()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.TENNIS_RACKET);
        }

        public Item MakeItemHockeyStick()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.HOCKEY_STICK);
        }

        public Item MakeItemMachete()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.MACHETE);
        }

        public Item MakeItemStandardAxe()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.STANDARD_AXE);
        }

        public Item MakeItemPickaxe()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.PICKAXE);
        }

        public Item MakeItemPipeWrench()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.PIPE_WRENCH);
        }

        //@@MP (Release 4)
        public Item MakeItemMolotov()
        {
            return new ItemGrenade(m_Game.GameItems.MOLOTOV, m_Game.GameItems.MOLOTOV_PRIMED);
        }

        public Item MakeItemDynamite()
        {
            return new ItemGrenade(m_Game.GameItems.DYNAMITE, m_Game.GameItems.DYNAMITE_PRIMED)
            {
                IsForbiddenToAI = true //@@MP (Release 5-5)
                //they're so rare we want them for the player only.
                //ai can't use them anyway because dynamite must be deployed within the blast radius, which goes against BehaviorThrowGrenade()
            };
        }

        public Item MakeItemBeer()
        {
            int quantity;
            ItemMedicineModel alcoholType;
            int roll = m_Game.Rules.Roll(0, 4);
            switch (roll)
            {
                case 0: alcoholType = m_Game.GameItems.ALCOHOL_BEER_BOTTLE_BROWN; quantity = 6; break;
                case 1: alcoholType = m_Game.GameItems.ALCOHOL_BEER_BOTTLE_GREEN; quantity = 6; break;
                case 2: alcoholType = m_Game.GameItems.ALCOHOL_BEER_CAN_BLUE; quantity = 6; break;
                case 3: alcoholType = m_Game.GameItems.ALCOHOL_BEER_CAN_RED; quantity = 6; break;
                default:
                    throw new InvalidOperationException("unhandled roll");
            }

            return new ItemMedicine(alcoholType)
            {
                Quantity = quantity
            };
        }

        public Item MakeItemCigarettes()
        {
            return new ItemMedicine(m_Game.GameItems.CIGARETTES)
            {
                Quantity = 20
            };
        }

        public Item MakeItemWildBerries()
        {
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.WILD_BERRIES.BestBeforeDays);
            return new ItemFood(m_Game.GameItems.WILD_BERRIES, freshUntil, false, false)
            {
                Quantity = 3
            };
        }

        //@@MP (Release 5-1)
        public Item MakeItemNailGun()
        {
            return new ItemRangedWeapon(m_Game.GameItems.NAIL_GUN);
        }

        public Item MakeItemNailGunAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_NAILS);
        }

        //@@MP (Release 5-5)
        public Item MakeItemVegetables()
        {
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.VEGETABLES.BestBeforeDays);
            return new ItemFood(m_Game.GameItems.VEGETABLES, freshUntil, false, false)
            {
                Quantity = 3
            };
        }

        public Item MakeItemVegetableSeeds()
        {
            return new Item(m_Game.GameItems.VEGETABLE_SEEDS)
            {
                IsForbiddenToAI = true
            };
        }

        //@@MP (Release 6-3)
        public Item MakeItemNightVisionGoggles()
        {
            return new ItemLight(m_Game.GameItems.NIGHT_VISION); //code under DoTakeItem() will switch to male type if required
        }

        public Item MakeItemC4Explosive()
        {
            return new ItemExplosive(m_Game.GameItems.C4,m_Game.GameItems.C4_PRIMED)
            {
                IsForbiddenToAI = true  //@@MP (Release 7-6)
            };
        }

        //@@MP (Release 7-1)
        public Item MakeItemSnackBar()
        {
            return new ItemFood(m_Game.GameItems.SNACK_BAR)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.SNACK_BAR.StackingLimit)
            };
        }

        public Item MakeItemFireHazardSuit()
        {
            return new ItemBodyArmor(m_Game.GameItems.FIRE_HAZARD_SUIT)
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemEnergyDrink()
        {
            return new ItemMedicine(m_Game.GameItems.ENERGY_DRINK)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.ENERGY_DRINK.StackingLimit)
            };
        }

        public Item MakeItemBinoculars()
        {
            return new ItemLight(m_Game.GameItems.BINOCULARS) //code under DoTakeItem() will switch to male type if required
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemSiphonKit()
        {
            return new Item(m_Game.GameItems.SIPHON_KIT)
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemFuelAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_FUEL);
        }

        public Item MakeItemChainsaw()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.CHAINSAW)
            {
                IsForbiddenToAI = true //AI can't use them because they need fuel cans for ammo, which is currently only handled for the player. TODO in the future?
            };
        }

        public Item MakeItemFlamethrower()
        {
            return new ItemRangedWeapon(m_Game.GameItems.FLAMETHROWER)
            {
                Ammo = 0
            };
        }

        public Item MakeItemCandlesBox()
        {
            return new Item(m_Game.GameItems.CANDLES_BOX)
            {
                Quantity = 40,
                IsForbiddenToAI = true //TODO in the future?
            };
        }

        public Item MakeItemFlaresKit()
        {
            return new Item(m_Game.GameItems.FLARES_KIT)
            {
                Quantity = 40,
                IsForbiddenToAI = true //TODO in the future?
            };
        }

        public Item MakeItemGlowsticksBox()
        {
            return new Item(m_Game.GameItems.GLOWSTICKS_BOX)
            {
                Quantity = 60,
                IsForbiddenToAI = true //TODO in the future?
            };
        }

        public Item MakeItemLitFlare()
        {
            return new ItemLight(m_Game.GameItems.LIGHT_FLARE);
        }

        public Item MakeItemLitGlowstick()
        {
            return new ItemLight(m_Game.GameItems.LIGHT_GLOWSTICK);
        }

        public Item MakeItemLiquorForMolotov()
        {
            int quantity;
            ItemModel alcoholType;
            int roll = m_Game.Rules.Roll(0, 2);
            switch (roll)
            {
                case 0: alcoholType = m_Game.GameItems.LIQUOR_AMBER; quantity = 6; break;
                case 1: alcoholType = m_Game.GameItems.LIQUOR_CLEAR; quantity = 6; break;
                default:
                    throw new InvalidOperationException("unhandled roll");
            }

            return new Item(alcoholType)
            {
                Quantity = quantity
            };
        }

        public Item MakeItemAlcohol()
        {
            Item alcoholType;
            if (m_Game.Rules.RollChance(66))
            {
                alcoholType = MakeItemBeer();
                return new ItemMedicine(alcoholType.Model)
                {
                    Quantity = alcoholType.Quantity
                };
            }
            else
            {
                alcoholType = MakeItemLiquorForMolotov();
                return new Item(alcoholType.Model)
                {
                    Quantity = alcoholType.Quantity
                };
            }
        }

        //@@MP (Release 7-2)
        public Item MakeItemPoliceRiotShield()
        {
            return new Item(m_Game.GameItems.POLICE_RIOT_SHIELD);
        }

        public Item MakeItemSmokeGrenade()
        {
            return new ItemGrenade(m_Game.GameItems.SMOKE_GRENADE, m_Game.GameItems.SMOKE_GRENADE_PRIMED)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.SMOKE_GRENADE.StackingLimit)
            };
        }

        public Item MakeItemFlashbang()
        {
            return new ItemGrenade(m_Game.GameItems.FLASHBANG, m_Game.GameItems.FLASHBANG_PRIMED)
            {
                Quantity = m_Game.GameItems.FLASHBANG.StackingLimit
            };
        }

        public Item MakeItemStunGun()
        {
            return new ItemRangedWeapon(m_Game.GameItems.STUN_GUN);
        }

        //@@MP (Release 7-3)
        public Item MakeItemSleepingBag()
        {
            return new Item(m_Game.GameItems.SLEEPING_BAG)
            {
                IsForbiddenToAI = true //TODO in the future?
            };
        }

        //@@MP (Release 7-6)
        public Item MakeItemSMG()
        {
            return new ItemRangedWeapon(m_Game.GameItems.SMG);
        }

        public Item MakeItemDoubleBarrel()
        {
            return new ItemRangedWeapon(m_Game.GameItems.DOUBLE_BARREL);
        }

        public Item MakeItemMinigun()
        {
            return new ItemRangedWeapon(m_Game.GameItems.MINIGUN)
            {
                IsForbiddenToAI = true //because there is only 1 per game, and because they can't be used in single-shot fire mode, only rapid fire
            };
        }

        public Item MakeItemMinigunAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_MINIGUN)
            {
                IsForbiddenToAI = true //because there is only 1 minigun per game
            };
        }

        public Item MakeItemTacticalShotgun()
        {
            return new ItemRangedWeapon(m_Game.GameItems.TACTICAL_SHOTGUN);
        }

        public Item MakeItemCleaver()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.CLEAVER);
        }

        public Item MakeItemBrassKnuckles()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.BRASS_KNUCKLES);
        }

        public Item MakeItemFlail()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.FLAIL);
        }

        public Item MakeItemKitchenKnife()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.KITCHEN_KNIFE);
        }

        public Item MakeItemScimitar()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SCIMITAR);
        }

        public Item MakeItemMace()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.MACE);
        }

        public Item MakeItemNunchaku()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.NUNCHAKU);
        }

        public Item MakeItemFryingPan()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.FRYING_PAN);
        }

        public Item MakeItemPitchFork()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.PITCH_FORK);
        }

        public Item MakeItemScythe()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SCYTHE);
        }

        public Item MakeItemSickle()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SICKLE);
        }

        public Item MakeItemSpear()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SPEAR);
        }

        public Item MakeItemSpikedMace()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.SPIKED_MACE);
        }

        public Item MakeItemFireAxe()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.FIRE_AXE);
        }

        public Item MakeItemArmyPrecisionRifle()
        {
            return new ItemRangedWeapon(m_Game.GameItems.ARMY_PRECISION_RIFLE);
        }

        public Item MakeItemGrenadeLauncher()
        {
            return new ItemRangedWeapon(m_Game.GameItems.GRENADE_LAUNCHER)
            {
                IsForbiddenToAI = true //because there is only 1 per game
            };
        }

        public Item MakeItemGrenadeLauncherAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_GRENADES)
            {
                IsForbiddenToAI = true //because there is only 1 launcher per game
            };
        }

        public Item MakeItemRandomCommonAmmo()
        {
            int randomItem = m_Game.Rules.Roll(0, 6);
            switch (randomItem)
            {
                case 0: return MakeItemHeavyRifleAmmo();
                case 1: return MakeItemPrecisionRifleAmmo();
                case 2: return MakeItemLightPistolAmmo();
                case 3: return MakeItemLightRifleAmmo();
                case 4: return MakeItemHeavyPistolAmmo();
                case 5: return MakeItemShotgunAmmo();
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeItemRandomAntiqueWeapon()
        {
            int roll = m_Game.Rules.Roll(0, 9);
            switch (roll)
            {
                case 0: return MakeItemFlail();
                case 1: return MakeItemScimitar();
                case 2: return MakeItemMace();
                case 3: return MakeItemSpikedMace();
                case 4: return MakeItemSpear();
                case 5: return MakeItemSickle();
                case 6: return MakeItemHolyHandGrenade();
                case 7: return MakeItemUniqueBookOfArmaments();
                case 8: return MakeItemKatana();
                default:
                    throw new InvalidOperationException("unhandled roll");
            }
        }

        public Item MakeItemHolyHandGrenade()
        {
            return new ItemGrenade(m_Game.GameItems.HOLY_HAND_GRENADE, m_Game.GameItems.HOLY_HAND_GRENADE_PRIMED)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.HOLY_HAND_GRENADE.StackingLimit)
            };
        }

        public Item MakeItemFishingRod()
        {
            return new Item(m_Game.GameItems.FISHING_ROD);
        }

        public Item MakeItemRawFish()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.RAW_FISH.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.RAW_FISH, freshUntil, true, true);
        }

        public Item MakeItemCookedFish()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.COOKED_FISH.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.COOKED_FISH, freshUntil, false, false);
        }

        public Item MakeItemRawRabbit()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.RAW_RABBIT.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.RAW_RABBIT, freshUntil, true, true);
        }

        public Item MakeItemCookedRabbit()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.COOKED_RABBIT.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.COOKED_RABBIT, freshUntil, false, false);
        }

        public Item MakeItemRawChicken()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.RAW_CHICKEN.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.RAW_CHICKEN, freshUntil, true, true);
        }

        public Item MakeItemCookedChicken()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.COOKED_CHICKEN.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.COOKED_CHICKEN, freshUntil, false, false);
        }

        public Item MakeItemRawDogMeat()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.RAW_DOG_MEAT.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.RAW_DOG_MEAT, freshUntil, true, true);
        }

        public Item MakeItemCookedDogMeat()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.COOKED_DOG_MEAT.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.COOKED_DOG_MEAT, freshUntil, false, false);
        }

        public Item MakeItemRawHumanFlesh()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.RAW_HUMAN_FLESH.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.RAW_HUMAN_FLESH, freshUntil, true, true);
        }

        public Item MakeItemCookedHumanFlesh()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.RAW_HUMAN_FLESH.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.COOKED_HUMAN_FLESH, freshUntil, false, false);
        }

        public Item MakeItemChickenEgg()
        {
            // FIXME: should be map local time.
            int timeNow = m_Game.Session.WorldTime.TurnCounter;
            int freshUntil = timeNow + (WorldTime.TURNS_PER_DAY * m_Game.GameItems.CHICKEN_EGG.BestBeforeDays);

            return new ItemFood(m_Game.GameItems.CHICKEN_EGG, freshUntil, false, false);
        }

        public Item MakeItemUniqueBookOfArmaments()
        {
            return new Item(m_Game.GameItems.UNIQUE_BOOK_OF_ARMAMENTS)
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemFireExtinguisher()
        {
            return new ItemSprayPaint(m_Game.GameItems.FIRE_EXTINGUISHER)
            {
                IsForbiddenToAI = true //@@MP - no point in them having these, as there's no code to handle it (Release 7-6)
            };
        }

        public Item MakeItemMatches()
        {
            return new Item(m_Game.GameItems.MATCHES)
            {
                Quantity = 20
            };
        }

        public Item MakeItemBioForceGun()
        {
            return new ItemRangedWeapon(m_Game.GameItems.BIO_FORCE_GUN)
            {
                IsForbiddenToAI = true //because there is only 1 per game
            };
        }

        public Item MakeItemBioForceGunAmmo()
        {
            return new ItemAmmo(m_Game.GameItems.AMMO_PLASMA)
            {
                IsForbiddenToAI = true, //because there is only 1 BFG per game
                Quantity = 1 //only ever spawn one at a time, to avoid it being too plentiful. it must be super rare
            };
        }

        public Item MakeItemBiohazardSuit()
        {
            return new ItemBodyArmor(m_Game.GameItems.BIOHAZARD_SUIT)
            {
                //IsForbiddenToAI = true,   //@@MP - removed, as CHAR scientists will wear them (Release 8-1)
            };
        }

        //@@MP (Release 8-1)
        public Item MakeItemBarbedWireBat()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.BARBED_WIRE_BAT);
        }

        public Item MakeItemKatana()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.KATANA);
        }

        public Item MakeItemKeyboard()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.KEYBOARD);
        }

        public Item MakeItemVintagePistol()
        {
            return new ItemRangedWeapon(m_Game.GameItems.VINTAGE_PISTOL);
        }

        public Item MakeItemBonesaw()  //replaces Jason Myer's axe
        {
            return new ItemMeleeWeapon(m_Game.GameItems.BONESAW)
            {
                IsUnique = true
            };
        }

        public Item MakeItemCHARLaptop()
        {
            return new Item(m_Game.GameItems.CHAR_LAPTOP);
        }

        //@@MP (Release 8-2)
        public Item MakeItemWaistPouch()
        {
            return new ItemBackpack(m_Game.GameItems.WAIST_POUCH)
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemSatchel()
        {
            return new ItemBackpack(m_Game.GameItems.SATCHEL)
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemDaypack()
        {
            return new ItemBackpack(m_Game.GameItems.DAYPACK)
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemHikingPack()
        {
            return new ItemBackpack(m_Game.GameItems.HIKING_PACK)
            {
                IsForbiddenToAI = true
            };
        }

        public Item MakeItemArmyRucksack()
        {
            return new ItemBackpack(m_Game.GameItems.ARMY_RUCKSACK)
            {
                IsForbiddenToAI = true
            };
        }
        #endregion

        #region Common tasks
        protected static void BarricadeDoors(Map map, Rectangle rect, int barricadeLevel) //@@MP - made static (Release 5-7)
        {
            barricadeLevel = Math.Min(Rules.BARRICADING_MAX, barricadeLevel);

            for (int x = rect.Left; x < rect.Right; x++)
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    DoorWindow door = map.GetMapObjectAt(x, y) as DoorWindow;
                    if (door == null)
                        continue;
                    door.BarricadePoints = barricadeLevel;
                }
        }
        #endregion

        #region Zones
        protected static Zone MakeUniqueZone(string basename, Rectangle rect) //@@MP - made static (Release 5-7)
        {
            string name = String.Format("{0}@{1}-{2}", basename, rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            return new Zone(name, rect);
        }
        #endregion
    }
}
