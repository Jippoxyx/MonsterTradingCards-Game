using MTCG.BL.Service;
using MTCG.DAL.Access;
using MTCG.Http;
using MTCG.Model;
using MTCG.Models;

namespace MTCG.Endpoint
{
    abstract class EndpointBase<T> 
    {
        public EndpointBase(T parameters) { }

        protected Response res = new Response();
        protected Request req = new Request();

        protected UserService userServ = new UserService();
        protected CardService cardServ = new CardService();
           
        protected UserAccess userAcc = new UserAccess();
        protected CardAccess cardAcc = new CardAccess();

        protected UserModel userObj = new UserModel();
        
        public virtual Response POST() 
        {
            res.Content = "Incoorectly request";
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }         
        public virtual Response GET()
        {
            res.Content = "Incoorectly request";
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }
        public virtual Response PUT()
        {
            res.Content = "Incoorectly request";
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }
        public virtual Response DELETE()
        {
            res.Content = "Incoorectly request";
            res.StatusCode = (int)HttpStatusCode.BadRequest;
            return res;
        }
    }
}