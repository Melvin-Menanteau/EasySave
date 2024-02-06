namespace EasySave
{
    public class UnitTest1
    {
        [Fact]
        public void TestSauvegardeComplète()
        {
            Save save = new Save
                (0,
                "Test",
                "C:\\Users\\vpetit\\Desktop\\test\\source",
                "C:\\Users\\vpetit\\Desktop\\test\\destination",
                SaveType.COMPLETE);

        }

        [Fact]
        public void TestSauvegardeDifférentielle()
        {

        }
    }
}