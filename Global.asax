<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    string _transferRequestPath;

    public void TransferRequest(string path) {
        // remember the path for later
        _transferRequestPath = path;

        // short circuit the pipeline by jumping to the
        // end request notifications where we can release
        // session state
        this.CompleteRequest();
    }

    // In Session_Start, we acquire session state.  This will
    // cause TransferRequest to hang unless we release session state first.
    void Session_Start() {

    }

    // By the time Application_EndRequest is called, session state has been released
    void Application_EndRequest() {

        // we may need to call TransferRequest
        if (_transferRequestPath != null) {

            // make copy of path and set instance field to null
            // since application instances are pooled and reused.

            string path = _transferRequestPath;
            _transferRequestPath = null;

            Context.Server.TransferRequest(path);
        }
    }
       
</script>
