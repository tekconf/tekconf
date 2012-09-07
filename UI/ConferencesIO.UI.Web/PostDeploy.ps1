$webClient = new-object System.Net.WebClient
$output = $webClient.DownloadString("http://conferencesio.cloudapp.net/")