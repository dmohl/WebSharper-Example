// This is a partial port of SharpCouch
// http://code.google.com/p/couchbrowse/source/browse/trunk/SharpCouch/SharpCouch.cs
module FSharpCouch
    open System
    open System.Net
    open System.Text
    open System.IO

    let ProcessRequest url methodName postData contentType =
        async { let request =  WebRequest.Create(string url)
                request.Method <- methodName
                request.ContentType <- contentType 
                let bytes = UTF8Encoding.UTF8.GetBytes(string postData)
                request.ContentLength <- bytes.LongLength
                use requestStream = request.GetRequestStream()
                requestStream.Write(bytes, 0, bytes.Length)
                use! response = request.AsyncGetResponse()
                use stream = response.GetResponseStream()
                use reader = new StreamReader(stream)
                let contents = reader.ReadToEnd()
                return contents }
        |> Async.RunSynchronously
    let CreateDocument url content = 
        ProcessRequest url "POST" content "application/json"