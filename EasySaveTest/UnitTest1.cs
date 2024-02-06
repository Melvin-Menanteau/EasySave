namespace EasySave
{
    public class UnitTest1
    {
        [Fact]
        public void EffectuerSauvegardeComplete_CopieContenuDossierSourceVersDossierCible()
        {
            // Arrange
            var easySaveC = new EasySaveC();
            string sourceDir = @"C:\Users\vpetit\Desktop\test\source";
            string targetDir = @"C:\Users\vpetit\Desktop\test\destination";

            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(Path.Combine(sourceDir, "subfolder1"));
            File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "Hello, world!");
            File.WriteAllText(Path.Combine(sourceDir, "subfolder1", "file2.txt"), "Bonjour!");

            // Act
            easySaveC.EffectuerSauvegardeComplete(new Save
                (0,
                "Test",
                sourceDir,
                targetDir,
                SaveType.COMPLETE));
            // Assert
            Assert.True(Directory.Exists(targetDir));
            Assert.True(File.Exists(Path.Combine(targetDir, "file1.txt")));
            Assert.True(File.Exists(Path.Combine(targetDir, "subfolder1", "file2.txt")));

            // Clean up
            Directory.Delete(sourceDir, true);
            Directory.Delete(targetDir, true);
        }

        [Fact]
        public void EffectuerSauvegardeDifferentielle_CopieFichiersModifiesOuAjoutes()
        {
            // Arrange
            var easySaveC = new EasySaveC();
            string sourceDir = @"C:\Users\vpetit\Desktop\test\source";
            string targetDir = "C:\\Users\\vpetit\\Desktop\\test\\destination";

            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(Path.Combine(sourceDir, "subfolder1"));
            File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "Hello, world!");
            File.WriteAllText(Path.Combine(sourceDir, "subfolder1", "file2.txt"), "Bonjour!");

            // Première sauvegarde
            easySaveC.EffectuerSauvegardeDifferentielle(new Save
                (0,
                "Test",
                sourceDir,
                targetDir,
                SaveType.COMPLETE));
            // Modifier un fichier et en ajouter un nouveau
            File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "Modified content");
            File.WriteAllText(Path.Combine(sourceDir, "file3.txt"), "New file content");

            // Act
            easySaveC.EffectuerSauvegardeDifferentielle(new Save
                (0,
                "Test",
                sourceDir,
                targetDir,
                SaveType.COMPLETE));
            // Assert
            Assert.True(File.Exists(Path.Combine(targetDir, "file1.txt")));
            Assert.True(File.Exists(Path.Combine(targetDir, "file3.txt")));
            Assert.True(File.Exists(Path.Combine(targetDir, "subfolder1", "file2.txt")));

            // Clean up
            Directory.Delete(sourceDir, true);
            Directory.Delete(targetDir, true);
        }
    }
}