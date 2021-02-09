# QueryMultipeDb
this package allow to get every context available in any class you passed in like params.
to use this package, it's quite simple
download it first and then afterwards 
in the class where your data context(s) is, add the using statement. After that, in the method of your choice 
call the static methods provided by the library


e.g:

using MultiDbQuery;

namespace QueryMoreDb.Services

{

    public class MainService : IMainService
    
    {
    
        public ILogger<MainService> _logger { get; }
        
        public bd1Context _bd1Context { get; }
        
        public Dbcontex2 _dbContext2 { get; }

        public MainService(bd1Context bd1Context,Dbcontex2 dbcontex2, ILogger<MainService> logger)
        {
            _bd1Context = bd1Context;
            _dbContext2 = dbcontex2;
            _logger = logger;
        }      
//get all student in oth 2 db
         public async Task<IEnumerable<Student>> GetAll()
        {
            var std = await AvailableContext.GetAll(this,new Student());
            return std;
        }
        
        //get all student in all 2 db with id =2
        public async Task<IEnumerable<Student>> GetByID(int id)
        {
            var std = await AvailableContext.Find(this,new Student(),(x=>x.Id == id));
            return std;
        }
    }
}




it is noted that the library provides some basic methods such as retrieving data from one or more data bases.

ex: GetAll(), FirstOrDefault(), Find()

we want to make it clear that the databases you want to request must have exactly the same database structure, which simply means that they must be identical, otherwise it won't work. Because the structure of the entities passed in parameters must match the structure of the tables in your database(s) exactly as in the entity framework (on which this library is based).

e.g. you might want to get the list of books from the old db and the new one. with this library this is simplified to the utmost by not needing too many lines of code.

the library is in charge of detecting the contexts of the available db and executing the request to return the desired data, you could even apply some filters to it.

it's extremely simple to use this library, it's based on entity frame work core to make requests.


