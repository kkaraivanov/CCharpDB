namespace IntroductionToDBApps
{
    using System.Collections.Generic;

    public class TablesPattern
    {
        public static Dictionary<string, string> Tables()
        {
            var dataBaseTables = new Dictionary<string, string>();
            string tableName = null;
            string filds = null;

            tableName = "Countries";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)";
            if (!dataBaseTables.ContainsKey(tableName))
                dataBaseTables[tableName] = null;
            dataBaseTables[tableName] = filds;

            tableName = "Towns";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)," +
                    "CountryCode INT FOREIGN KEY REFERENCES Countries(Id)";
            if (!dataBaseTables.ContainsKey(tableName))
                dataBaseTables[tableName] = null;
            dataBaseTables[tableName] = filds;

            tableName = "Minions";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(30)," +
                    "Age INT," +
                    "TownId INT FOREIGN KEY REFERENCES Towns(Id)";
            if (!dataBaseTables.ContainsKey(tableName))
                dataBaseTables[tableName] = null;
            dataBaseTables[tableName] = filds;

            tableName = "EvilnessFactors";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)";
            if (!dataBaseTables.ContainsKey(tableName))
                dataBaseTables[tableName] = null;
            dataBaseTables[tableName] = filds;

            tableName = "Villains";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)," +
                    "EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id)";
            if (!dataBaseTables.ContainsKey(tableName))
                dataBaseTables[tableName] = null;
            dataBaseTables[tableName] = filds;

            tableName = "MinionsVillains";
            filds = "MinionId INT FOREIGN KEY REFERENCES Minions(Id)," +
                    "VillainId INT FOREIGN KEY REFERENCES Villains(Id)," +
                    "CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId)";
            if (!dataBaseTables.ContainsKey(tableName))
                dataBaseTables[tableName] = null;
            dataBaseTables[tableName] = filds;

            return dataBaseTables;
        }
    }
}
