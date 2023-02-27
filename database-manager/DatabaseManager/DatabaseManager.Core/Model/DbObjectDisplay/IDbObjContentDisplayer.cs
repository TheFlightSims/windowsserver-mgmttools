namespace DatabaseManager.Model
{
    public interface IDbObjContentDisplayer
    {
        void Show(DatabaseObjectDisplayInfo displayInfo);
        ContentSaveResult Save(ContentSaveInfo saveInfo);
    }
}
