$webClient = new-object System.Net.WebClient
$output = $webClient.DownloadString("http://www.tekconf.com/")