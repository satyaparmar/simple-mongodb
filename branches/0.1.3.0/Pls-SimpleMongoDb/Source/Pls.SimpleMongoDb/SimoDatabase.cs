using Pls.SimpleMongoDb.Commands;

namespace Pls.SimpleMongoDb
{
    public class SimoDatabase
        : ISimoDatabase
    {
        public ISimoSession Session { get; private set; }
        
        public string Name { get; private set; }

        public ISimoCollection this[string name] 
        {
            get { return GetCollection(name); }
        }

        public SimoDatabase(ISimoSession session, string name)
        {
            Session = session;
            Name = name;
        }

        public void DropDatabase()
        {
            var cmd = new DatabaseCommand(Session.Connection)
            {
                DatabaseName = Name,
                Command = new { dropDatabase = 1 }
            };
            cmd.Execute();
        }

        public ISimoCollection GetCollection<T>()
            where T : class
        {
            var entityName = EntityMetadata<T>.EntityName;

            return GetCollection(entityName);
        }

        private ISimoCollection GetCollection(string name)
        {
            return new SimoCollection(this, name);
        }
    }
}