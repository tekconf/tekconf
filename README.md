GET http://conferencesioapi.azurewebsites.net/v1/conferences

  Returns list of conferences with urls to details
            
           <pre><code>
            [
                {
                    "name" : "That-Conference",
                    "start" : "2012-08-13",
                    "end" : "2012-08-15",
                    "location" : "Wisconsin",
                    "url" : "http://conferencesioapi.azurewebsites.net/v1/That-Conference/2012"
                },
            ]
            </code></pre>



GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012
    
    
    - Returns general info with link to sessions
        {
            _id: "498f66af7f9442e4b4be7ca6d3ec52e5",
            description: "Spend 3 days, with 1000 of your fellow campers in 150 sessions geeking out on everything Mobile, Web and Cloud at a giant waterpark.",
            facebookUrl: "https://www.facebook.com/CodeMash",
            slug: "CodeMash-2012",
            homepageUrl: "http://codemash.com",
            lanyrdUrl: "lanyrd.com/2012/codemash/",
            location: "Kalahari Resort, Wisconsin Dells, WI",
            name: "That Conference",
            start: "/Date(1344830400000-0400)/",
            end: "/Date(1345003200000-0400)/",
            twitterHashTag: "#codemash",
            twitterName: "@codemash",
            sessions : "http://conferencesioapi.azurewebsites.net/v1/CodeMash-2012/session"
        }

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/sessions
    - Returns sessions with speakers

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/sessions/MonoTouch
    - Returns single session with speakers, prerequisites, links, resources

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/sessions/MonoTouch/speakers
    - Returns speakers

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/sessions/MonoTouch/prerequisites
    - Returns prerequisites

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/sessions/MonoTouch/links
    - Returns links

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/sessions/MonoTouch/resources
    - Returns resources

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/speakers
    - Returns speakers with sessions

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012/speakers/rob-gibbens
    - Returns speakers info

GET http://conferencesioapi.azurewebsites.net/v1/conferences/That-Conference/2012

- Authenticate
- Get schedule
- Add session to schedule
- Add conference
- Add session
- Add new speaker
- Add existing speaker
- Remove session from schedule
- Submit session
- Approve session
- Vote on session
- Reject session
- Remove session
- Edit/update session
- Edit/update conference
- Edit/update speaker



<div style="background: #00578e url('http://www.jetbrains.com/img/banners/Codebetter300x250.png') no-repeat 0 50%; margin:0;padding:0;text-decoration:none;text-indent:0;letter-spacing:-0.001em; width:300px; height:250px">
<a href="http://www.jetbrains.com/youtrack" title="YouTrack by JetBrains" style="margin: 52px 0 0 58px;padding: 0; float: left;font-size: 14px; background-image:none;border:0;color: #acc4f9; font-family: trebuchet ms,arial,sans-serif;font-weight: normal;text-align:left;">keyboard-centric bug tracker</a>
<a href="http://www.jetbrains.com/teamcity" title="TeamCity by JetBrains" style="margin:0 0 0 58px;padding:122px 0 0 0;font-size:14px; background-image:none;border:0;display:block; color: #acc4f9; font-family: trebuchet ms,arial,sans-serif;font-weight: normal;text-align:left;">continuous integration server</a>
</div>