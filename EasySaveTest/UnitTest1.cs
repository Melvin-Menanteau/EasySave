namespace EasySave
{
    public class UnitTest1
    {
        [Fact]
        public void TestSauvegardeCompl�te()
        {
            Save save = new Save
                (0,
                "Test",
                "C:\\Users\\vpetit\\Desktop\\test\\source",
                "C:\\Users\\vpetit\\Desktop\\test\\destination",
                SaveType.COMPLETE);

        }

        [Fact]
        public void TestSauvegardeDiff�rentielle()
        {

        }
    }
}