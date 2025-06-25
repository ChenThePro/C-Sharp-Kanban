namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal interface IDTO
    {
        void Update(string column, object newValue);

        string[] GetColumnNames();
        
        object[] GetColumnValues();
    }
}
