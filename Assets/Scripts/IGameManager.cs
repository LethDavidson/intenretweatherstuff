public interface IGameManager {
	ManagerStatus status {get;}

	void Startup(NetworkService service); // startup function now takes one paramaeter, the injected object. 
}
