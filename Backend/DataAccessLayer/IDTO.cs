namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal interface IDTO
    {
        string[] GetColumnNames();
        object[] GetColumnValues();
    }
}
