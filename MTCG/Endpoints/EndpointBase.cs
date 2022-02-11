using MTCG.DAL.Access;
using MTCG.Http;
namespace MTCG.Handlers
{
    abstract class EndpointBase<T> 
    {
        public EndpointBase(T parameters) { }

        public Response res = new Response();
        public Request req = new Request();
        public UserAccess userAcc = new UserAccess();

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