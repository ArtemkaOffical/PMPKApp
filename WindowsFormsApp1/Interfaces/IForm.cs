namespace PMPK.Interfaces
{
    interface IForm
    {
        //Метод для проверки возможности сохранения
        public bool CanSave();

        //Метод для сохранения
        public void Save();

        //Метод для удаления
        public void Delete();
    }
}
