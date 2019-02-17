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
        static readonly string[] MALE_SHOES = new string[] { GameImages.MALE_SHOES1, GameImages.MALE_SHOES2, GameImages.MALE_SHOES3 };
        static readonly string[] MALE_EYES = new string[] { GameImages.MALE_EYES1, GameImages.MALE_EYES2, GameImages.MALE_EYES3, GameImages.MALE_EYES4, GameImages.MALE_EYES5, GameImages.MALE_EYES6 };

        static readonly string[] FEMALE_SKINS = new string[] { GameImages.FEMALE_SKIN1, GameImages.FEMALE_SKIN2, GameImages.FEMALE_SKIN3, GameImages.FEMALE_SKIN4, GameImages.FEMALE_SKIN5 };
        static readonly string[] FEMALE_HEADS = new string[] { GameImages.FEMALE_HAIR1, GameImages.FEMALE_HAIR2, GameImages.FEMALE_HAIR3, GameImages.FEMALE_HAIR4, GameImages.FEMALE_HAIR5, GameImages.FEMALE_HAIR6, GameImages.FEMALE_HAIR7 };
        static readonly string[] FEMALE_TORSOS = new string[] { GameImages.FEMALE_SHIRT1, GameImages.FEMALE_SHIRT2, GameImages.FEMALE_SHIRT3, GameImages.FEMALE_SHIRT4 };
        static readonly string[] FEMALE_LEGS = new string[] { GameImages.FEMALE_PANTS1, GameImages.FEMALE_PANTS2, GameImages.FEMALE_PANTS3, GameImages.FEMALE_PANTS4, GameImages.FEMALE_PANTS5 };
        static readonly string[] FEMALE_SHOES = new string[] { GameImages.FEMALE_SHOES1, GameImages.FEMALE_SHOES2, GameImages.FEMALE_SHOES3 };
        static readonly string[] FEMALE_EYES = new string[] { GameImages.FEMALE_EYES1, GameImages.FEMALE_EYES2, GameImages.FEMALE_EYES3, GameImages.FEMALE_EYES4, GameImages.FEMALE_EYES5, GameImages.FEMALE_EYES6 };

        static readonly string[] BIKER_HEADS = new string[] { GameImages.BIKER_HAIR1, GameImages.BIKER_HAIR2, GameImages.BIKER_HAIR3 };
        static readonly string[] BIKER_LEGS = new string[] { GameImages.BIKER_PANTS };
        static readonly string[] BIKER_SHOES = new string[] { GameImages.BIKER_SHOES };

        static readonly string[] CHARGUARD_HEADS = new string[] { GameImages.CHARGUARD_HAIR };
        static readonly string[] CHARGUARD_LEGS = new string[] { GameImages.CHARGUARD_PANTS };

        static readonly string[] DOG_SKINS = new string[] { GameImages.DOG_SKIN1, GameImages.DOG_SKIN2, GameImages.DOG_SKIN3 };

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
        #endregion

        #region Actor naming helpers
        static readonly string[] MALE_FIRST_NAMES = 
        {
            "Aaron", "Adam", "Adrian", "Alan", "Albert", "Alberto", "Alex", "Alexander", "Alfred", "Alfredo", "Allan", "Allen", "Alvin", "Andre", "Andrew", "Andy", "Angel", "Anton", "Antonio", "Anthony", "Armando", "Arnold", "Arthur", "Ashley", "Axel",
            "Barry", "Ben", "Benjamin", "Bernard", "Bill", "Billy", "Bob", "Bobby", "Brad", "Brandon", "Bradley", "Brent", "Brett", "Brian", "Bryan", "Bruce", "Byron",
            "Caine", "Calvin", "Carl", "Carlos", "Carlton", "Casey", "Cecil", "Chad", "Charles", "Charlie", "Chester", "Chris", "Christian", "Christopher", "Clarence", "Clark", "Claude", "Clayton", "Clifford", "Clifton", "Clinton", "Clyde", "Cody", "Corey", "Cory", "Craig", "Cris", "Cristobal", "Curtis",
            "Dan", "Daniel", "Danny", "Dale", "Darrell", "Darren", "Darryl", "Dave", "David", "Dean", "Dennis", "Derek", "Derrick", "Dirk", "Don", "Donald", "Donovan", "Doug", "Douglas", "Duane", "Dustin", "Dwayne", "Dwight",
            "Earl", "Ed", "Eddie", "Eddy", "Edgar", "Eduardo", "Edward", "Edwin", "Elias", "Elie", "Elmer", "Elton", "Enrique", "Eric", "Erik", "Ernest", "Eugene", "Everett",
            "Felix", "Fernando", "Floyd", "Francis", "Francisco", "Frank", "Franklin", "Fred", "Frederick", "Freddie",
            "Gabriel", "Gary", "Gene", "George", "Georges", "Gerald", "Gilbert", "Glenn", "Gordon", "Greg", "Gregory", "Guy",
            "Hank", "Harold", "Harvey", "Harry", "Hector", "Henry", "Herbert", "Herman", "Howard", "Hubert", "Hugh", "Hughie",
            "Ian", "Indy", "Isaac", "Ivan",
            "Jack", "Jacob", "Jaime", "Jake", "James", "Jamie", "Jared", "Jarvis", "Jason", "Javier", "Jay", "Jeff", "Jeffrey", "Jeremy", "Jerome", "Jerry", "Jesse", "Jessie", "Jesus", "Jim", "Jimmie", "Jimmy", "Joe", "Joel", "John", "Johnnie", "Johnny", "Jon", "Jonas", "Jonathan", "Jordan", "Jorge", "Jose", "Joseph", "Joshua", "Juan", "Julian", "Julio", "Justin",
            "Karl", "Keith", "Kelly", "Ken", "Kenneth", "Kent", "Kevin", "Kirk", "Kurt", "Kyle",
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
            "Xavier",
            // Y
            "Zachary"
        };

        static readonly string[] FEMALE_FIRST_NAMES = 
        {
            "Abigail", "Agnes", "Ali", "Alice", "Alicia", "Allison", "Alma", "Amanda", "Amber", "Amy", "Andrea", "Angela", "Anita", "Ana", "Ann", "Anna", "Anne", "Annette", "Annie", "April", "Arlene", "Ashley", "Audrey",
            "Barbara", "Beatrice", "Becky", "Belinda", "Bernice", "Bertha", "Bessie", "Beth", "Betty", "Beverly", "Billie", "Bobbie", "Bonnie", "Brandy", "Brenda", "Britanny",
            "Carla", "Carmen", "Carol", "Carole", "Caroline", "Carolyn", "Carrie", "Cassandra", "Cassie", "Cathy", "Catherine", "Charlene", "Charlotte", "Cherie", "Cheryl", "Christina", "Christine", "Christy", "Cindy", "Claire", "Clara", "Claudia", "Colleen", "Connie", "Constance", "Courtney", "Cris", "Crissie", "Crystal",  "Cynthia",
            "Daisy", "Dana", "Danielle", "Darlene", "Dawn", "Deanna", "Debbie", "Deborah", "Debrah", "Delores", "Denise", "Diana", "Diane", "Donna", "Dolores", "Dora", "Doris", "Dorothy",
            "Edith", "Edna", "Eileen", "Elaine", "Elayne", "Eleanor", "Eleonor", "Elizabeth", "Ella", "Ellen", "Elsie", "Emily", "Emma", "Erica", "Erika", "Erin", "Esther", "Ethel", "Eva", "Evelyn",
            "Felicia", "Fiona", "Florence", "Fran", "Frances",
            "Gail", "Georgia", "Geraldine", "Gertrude", "Gina", "Ginger", "Gladys", "Glenda", "Gloria", "Grace", "Gwendolyn",
            "Hazel", "Heather", "Heidi", "Helen", "Helena", "Hilary", "Hilda", "Holly", "Holy",
            "Ida", "Ingrid", "Irene", "Irma", "Isabela",
            "Jackie", "Jacqueline", "Jamie", "Jane", "Janet", "Janice", "Jean", "Jeanne", "Jeanette", "Jennie", "Jennifer", "Jenny", "Jess", "Jessica", "Jessie", "Jill", "Jo", "Joan", "Joana", "Joanne", "Josephine", "Joy", "Joyce", "Juanita", "Judith", "Judy", "Julia", "Julie", "June",
            "Karen", "Kate", "Katherine", "Kathleen", "Kathy", "Kathryn", "Katie", "Katrina", "Kay", "Kelly", "Kim", "Kimberly", "Kira", "Kristen", "Kristin", "Kristina",
            "Laura", "Lauren", "Laurie", "Lea", "Lena", "Leona", "Leonor", "Leslie", "Lillian", "Lillie", "Linda", "Lindsay", "Lisa", "Liz", "Lois", "Loretta", "Lori", "Lorraine", "Louise", "Lucia", "Lucille", "Lucy", "Lydia", "Lynn",
            "Mabel", "Mae", "Maggie", "Marcia", "Margaret", "Margie", "Maria", "Marian", "Marie", "Marion", "Marjorie", "Marlene", "Marsha", "Martha", "Mary", "Marylin", "Mary-Ann", "Mattie", "Maureen", "Maxine", "Megan", "Melanie", "Melinda", "Melissa", "Michele", "Mildred", "Millie", "Minnie", "Miriam", "Misty", "Molly", "Monica", "Myrtle",
            "Naomi", "Nancy", "Natalie", "Nelly", "Nicole", "Nina", "Nora", "Norma",
            "Olga", "Ophelia",
            "Paquita", "Page", "Pamela", "Patricia", "Patsy", "Patty", "Paula", "Pauline", "Pearl", "Peggy", "Penny", "Phyllis", "Priscilla",
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
            "Adams", "Alexander", "Allen","Anderson", "Austin","Alsop","Andrews","Ablett",
            "Bailey", "Baker", "Barnes", "Bell", "Bennett", "Black", "Bradley", "Brown", "Brooks", "Bryant", "Bush", "Butler", "Brewster",
            "Campbell", "Carpenter", "Carter", "Clark", "Coleman", "Collins", "Cook", "Cooper", "Cordell", "Cox", "Chen","Carpenter","Camden",
            "Davis", "Diaz", "Dobbs","Davies","Dawson",
            "Edwards", "Engels", "Evans","Engels","Epstein","Ericson","Ellis",
            "Finch", "Flores", "Ford", "Forrester", "Foster", "Frampton",
            "Garcia", "Gates", "Gonzales", "Gonzalez", "Gray", "Green", "Griffin","Gavins","Gregson","Granger","Godfreys",
            "Hall", "Harris", "Hayes", "Henderson", "Hernandez", "Hewlett", "Hill", "Holtz", "Howard", "Hughes","Holtz","Himmel","Hampson","Harper",
            "Irvin","Ipswitch",
            "Jackson", "James", "Jenkins", "Johnson", "Jones","Jameson","Jefferson","Johnstone","Jacobs",
            "Kelly", "Kennedy", "King","Kevins",
            "Lambert", "Lesaint", "Lee", "Lewis", "Long", "Lopez",
            "Malory", "Martin", "Martinez", "McAllister", "McGready", "Miller", "Mitchell", "Moore", "Morgan", "Morris", "Murphy","McAllister","Maynard",
            "Nelson", "Norton", "Newnes","Nguyen",
            "O'Brien", "Oswald", "Orson",
            "Parker", "Patterson", "Paul", "Perez", "Perry", "Peterson", "Phillips", "Powell", "Price", "Paulson", "Pitt","Peters",
            "Quinn",
            "Ramirez", "Reed", "Reeves", "Richardson", "Rivera", "Roberts", "Robinson", "Rockwell", "Rodriguez", "Rogers", "Robertson", "Ross", "Russell", "Robertson","Reynolds",
            "Sanchez", "Sanders", "Scott", "Simmons", "Smith", "Stevens", "Stewart","Sanchez", "Steward","Sampson",
            "Tarver", "Taylor", "Thomas", "Thompson", "Torres", "Turner", "Tulev",
            "Ulrich","Ulman",
            "Vance","Vermont","Vincent",
            "Walker", "Ward", "Walters", "Washington", "Watson", "White", "Williams", "Wilson", "Wood", "Wright",
            "Xiao",
            "Young"
            // Z
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
        protected static DoorWindow MakeObjWoodenDoor() //@@MP - made static (Release 5-7)
        {
            return new DoorWindow("wooden door", GameImages.OBJ_WOODEN_DOOR_CLOSED, GameImages.OBJ_WOODEN_DOOR_OPEN, GameImages.OBJ_WOODEN_DOOR_BROKEN, DoorWindow.BASE_HITPOINTS)
            {
                GivesWood = true
            };
        }

        protected static DoorWindow MakeObjHospitalDoor() //@@MP - made static (Release 5-7)
        {
            return new DoorWindow("door", GameImages.OBJ_HOSPITAL_DOOR_CLOSED, GameImages.OBJ_HOSPITAL_DOOR_OPEN, GameImages.OBJ_HOSPITAL_DOOR_BROKEN, DoorWindow.BASE_HITPOINTS)
            {
                GivesWood = true
            };
        }

        protected static DoorWindow MakeObjCharDoor() //@@MP - made static (Release 5-7)
        {
            return new DoorWindow("CHAR door", GameImages.OBJ_CHAR_DOOR_CLOSED, GameImages.OBJ_CHAR_DOOR_OPEN, GameImages.OBJ_CHAR_DOOR_BROKEN, 4 * DoorWindow.BASE_HITPOINTS)
            {
                GivesWood = true
            };
        }

        protected static DoorWindow MakeObjGlassDoor() //@@MP - made static (Release 5-7)
        {
            return new DoorWindow("glass door", GameImages.OBJ_GLASS_DOOR_CLOSED, GameImages.OBJ_GLASS_DOOR_OPEN, GameImages.OBJ_GLASS_DOOR_BROKEN, DoorWindow.BASE_HITPOINTS / 4)
            {
                IsMaterialTransparent = true,
                BreaksWhenFiredThrough = true
            };
        }

        public DoorWindow MakeObjIronDoor() //@@MP - made public (Release 6-3)
        {
            return new DoorWindow("iron door", GameImages.OBJ_IRON_DOOR_CLOSED, GameImages.OBJ_IRON_DOOR_OPEN, GameImages.OBJ_IRON_DOOR_BROKEN, 8 * DoorWindow.BASE_HITPOINTS)
            {
                IsAn = true,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static MapObject MakeObjLockedDoor(string doorImageID) //@@MP (Release 4), made static (Release 5-7)
        {
            return new MapObject("locked door", doorImageID)
            {
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static DoorWindow MakeObjRollerDoor() //@@MP (Release 4), made static (Release 5-7)
        {
            return new DoorWindow("roller door", GameImages.OBJ_ROLLER_DOOR_CLOSED, GameImages.OBJ_ROLLER_DOOR_OPEN, GameImages.OBJ_ROLLER_DOOR_BROKEN, 6 * DoorWindow.BASE_HITPOINTS)
            {
                IsMetal = true, //@@MP (Release 5-4)
            };
        }

        protected static DoorWindow MakeObjWindow() //@@MP - made static (Release 5-7)
        {
            // windows as transparent doors.
            return new DoorWindow("window", GameImages.OBJ_WINDOW_CLOSED, GameImages.OBJ_WINDOW_OPEN, GameImages.OBJ_WINDOW_BROKEN, DoorWindow.BASE_HITPOINTS / 4)
            {
                IsWindow = true,
                IsMaterialTransparent = true,
                //GivesWood = true, //@@MP (Release 5-3)
                BreaksWhenFiredThrough = true
            };
        }

        protected static MapObject MakeObjFence(string fenceImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("chain wire fence", fenceImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 10) //@@MP - appended 'chain wire' for clarity (Release 5-4)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                //GivesWood = true, //@@MP - metal fences shouldn't give wood (Release 5-4)
                IsMetal = true, //@@MP (Release 5-4)
                StandOnFovBonus = true
            };
        }

        protected static MapObject MakeObjWoodenFence(string fenceImageID) //@@MP (Release 6-1)
        {
            return new MapObject("wooden fence", fenceImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                StandOnFovBonus = true
            };
        }

        protected static MapObject MakeObjIronFence(string fenceImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("iron fence", fenceImageID)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1, //@@MP (Release 5-6)
                IsAn = true,
                IsMetal = true, //@@MP (Release 5-4)
                StandOnFovBonus = true //@@MP (Release 5-6)
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

        protected static MapObject MakeObjTree(string treeImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("tree", treeImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 20) //@@MP - made unbreakable (Release 6-2)
            {
                GivesWood = true
            };
        }

        static string[] CARS = { GameImages.OBJ_CAR1, GameImages.OBJ_CAR2, GameImages.OBJ_CAR3, GameImages.OBJ_CAR4 };

        /// <summary>
        /// Makes a new wrecked car of a random model.
        /// <see>MakeWreckedCar(string)</see>
        /// </summary>
        /// <param name="roller"></param>
        /// <returns></returns>
        protected static MapObject MakeObjWreckedCar(DiceRoller roller) //@@MP - made static (Release 5-7)
        {
            return MakeObjWreckedCar(CARS[roller.Roll(0, CARS.Length)]);
        }

        /// <summary>
        /// Makes a new wrecked car : transparent, not walkable but jumpable, movable.
        /// </summary>
        /// <param name="carImageID"></param>
        /// <returns></returns>
        protected static MapObject MakeObjWreckedCar(string carImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("wrecked car", carImageID)
            {
                BreakState = MapObject.Break.BROKEN,
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsMovable = true,
                Weight = 100,
                StandOnFovBonus = true,
                IsMetal = true //@@MP (Release 5-4)
            };
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

        protected static MapObject MakeObjBench(string benchImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("bench", benchImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                IsCouch = true,
                GivesWood = true
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
                IsMaterialTransparent = false,
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
                Weight = 2
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
            return new MapObject("junk", junkImageID, MapObject.Break.BREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsPlural = true,
                IsMaterialTransparent = true,
                IsMovable = true,
                IsContainer = true, //@@MP (Release 5-3)
                GivesWood = true,
                Weight = 6
            };
        }

        protected static MapObject MakeObjBarrels(string barrelsImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("barrels", barrelsImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.UNINFLAMMABLE, DoorWindow.BASE_HITPOINTS * 2) //@@MP - made unbreakable (Release 6-2)
            {
                IsPlural = true,
                IsMaterialTransparent = true,
                IsMovable = true,
                IsContainer = true, //@@MP (Release 5-3)
                GivesWood = true,
                Weight = 10,
                IsMetal = true //@@MP (Release 5-4)
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
            return new MapObject("CHAR vat", vatImageID);
        }

        protected static MapObject MakeObjCHARdesktop(string CHARdesktopImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("CHAR desktop", CHARdesktopImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                IsMovable = true,
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
                //JumpLevel = 1, //@@MP (Release 5-3)
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
                Weight = 3
            };
        }

        protected static MapObject MakeObjKitchenCounter(string kitchenCounterImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("counter", kitchenCounterImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 4)
            {
                IsContainer = true,
                //JumpLevel = 1, //@@MP (Release 5-3)
                IsMaterialTransparent = true,
                GivesWood = true
            };
        }

        protected static MapObject MakeObjBarCounter(string barCounterImageID) //@@MP (Release 5-3), made static (Release 5-7)
        {
            return new MapObject("counter", barCounterImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 4)
            {
                JumpLevel = 1,
                IsMaterialTransparent = true,
                GivesWood = true
            };
        }

        protected static MapObject MakeObjCashRegister(string cashRegisterImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("cash register", cashRegisterImageID)
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

        protected static MapObject MakeObjBankSafe(string banksafeImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("bank safe", banksafeImageID)
            {
                IsContainer = true,
                IsMetal = true //@@MP (Release 5-4)
            };
        }

        protected static MapObject MakeObjBankTeller(string banktellerImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("bank teller", banktellerImageID)
            {
                IsMaterialTransparent = true, //@@MP (Release 5-3)
            };
        }

        protected static MapObject MakeObjBerryBush(string berrybushImageID) //@@MP - made static (Release 5-7)
        {
            return new MapObject("berry bush", berrybushImageID, MapObject.Break.UNBREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS / 20)
            {
                IsMaterialTransparent = true,
                IsContainer = true
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
        protected static MapObject MakeObjArmyComputerStation(string armyDesktopImageID)
        {
            return new MapObject("Army workstation", armyDesktopImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS)
            {
                IsMaterialTransparent = true,
                JumpLevel = 1,
                GivesWood = true,
                IsMovable = true,
            };
        }
        protected static MapObject MakeObjArmyRadioCupboard(string armyRadioCupboardImageID)
        {
            return new MapObject("radio equipment", armyRadioCupboardImageID, MapObject.Break.BREAKABLE, MapObject.Fire.BURNABLE, DoorWindow.BASE_HITPOINTS * 2)
            {
                IsMaterialTransparent = false,
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

        public MapObject MakeObjHelicopter(string vatImageID) //@@MP (Release 6-4)
        {
            return new MapObject("helicopter", vatImageID);
            //A 64x128 pixel image, dissected into 8 pieces and then arranged as:
            //  1234
            //  5678
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
        public Item MakeItemBandages()
        {
            return new ItemMedicine(m_Game.GameItems.BANDAGE)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.BANDAGE.StackingLimit)
            };
        }

        public Item MakeItemMedikit()
        {
            return new ItemMedicine(m_Game.GameItems.MEDIKIT);
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

            int max = WorldTime.TURNS_PER_DAY * m_Game.GameItems.GROCERIES.BestBeforeDays;
            int min = max / 2;
            int freshUntil = timeNow + m_Rules.Roll(min, max);

            return new ItemFood(m_Game.GameItems.GROCERIES, freshUntil);
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

        public Item MakeItemJasonMyersAxe()
        {
            return new ItemMeleeWeapon(m_Game.GameItems.UNIQUE_JASON_MYERS_AXE)
            {
                IsUnique = true
            };
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

        public Item MakeItemKoltRevolver()
        {
            return new ItemRangedWeapon(m_Game.GameItems.KOLT_REVOLVER);
        }

        public Item MakeItemRandomPistol()
        {
            return m_Game.Rules.RollChance(50) ? MakeItemPistol() : MakeItemKoltRevolver();
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

            return new ItemSprayPaint(paintModel);
        }

        public Item MakeItemStenchKiller()
        {
            return new ItemSprayScent(m_Game.GameItems.STENCH_KILLER);
        }

        public Item MakeItemArmyRifle()
        {
            return new ItemRangedWeapon(m_Game.GameItems.ARMY_RIFLE);
        }

        public Item MakeItemPrecisionRifle()
        {
            return new ItemRangedWeapon(m_Game.GameItems.PRECISION_RIFLE);
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

        public Item MakeItemBook()
        {
            return new ItemEntertainment(m_Game.GameItems.BOOK);
        }

        public Item MakeItemMagazines()
        {
            return new ItemEntertainment(m_Game.GameItems.MAGAZINE)
            {
                Quantity = m_Rules.Roll(1, m_Game.GameItems.MAGAZINE.StackingLimit)
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
            };
        }

        public Item MakeItemAlcohol()
        {
            int quantity;
            ItemMedicineModel alcoholType;
            int roll = m_Game.Rules.Roll(0, 6);
            switch (roll)
            {
                case 0: alcoholType = m_Game.GameItems.ALCOHOL_BEER_BOTTLE_BROWN; quantity = 6; break;
                case 1: alcoholType = m_Game.GameItems.ALCOHOL_BEER_BOTTLE_GREEN; quantity = 6; break;
                case 2: alcoholType = m_Game.GameItems.ALCOHOL_BEER_CAN_BLUE; quantity = 6; break;
                case 3: alcoholType = m_Game.GameItems.ALCOHOL_BEER_CAN_RED; quantity = 6; break;
                case 4: alcoholType = m_Game.GameItems.ALCOHOL_LIQUOR_AMBER; quantity = 3; break;
                case 5: alcoholType = m_Game.GameItems.ALCOHOL_LIQUOR_CLEAR; quantity = 3; break;
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
            return new ItemFood(m_Game.GameItems.WILD_BERRIES, freshUntil)
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
            return new ItemFood(m_Game.GameItems.VEGETABLES, freshUntil)
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
            return new ItemLight(m_Game.GameItems.NIGHT_VISION_FEMALE); //code under DoTakeItem() will switch to male type if required
        }

        public Item MakeItemC4Explosive()
        {
            return new ItemExplosive(m_Game.GameItems.C4,m_Game.GameItems.C4_PRIMED);
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
