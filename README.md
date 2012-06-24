GET http://conferences.arteksoftware.com

  Returns list of conferences with urls to details
            
           <pre><code>
            [
                {
                    "name" : "That-Conference",
                    "start" : "2012-08-13",
                    "end" : "2012-08-15",
                    "location" : "Wisconsin",
                    "url" : "http://conferences.arteksoftware.com/That-Conference/2012"
                },
            ]
            </code></pre>



GET http://conferences.arteksoftware.com/That-Conference/2012
    
    
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
            sessions : "http://conferences.arteksoftware.com/CodeMash-2012/session"
        }

GET http://conferences.arteksoftware.com/That-Conference/2012/sessions
    - Returns sessions with speakers

GET http://conferences.arteksoftware.com/That-Conference/2012/sessions/MonoTouch
    - Returns single session with speakers, prerequisites, links, resources

GET http://conferences.arteksoftware.com/That-Conference/2012/sessions/MonoTouch/speakers
    - Returns speakers

GET http://conferences.arteksoftware.com/That-Conference/2012/sessions/MonoTouch/prerequisites
    - Returns prerequisites

GET http://conferences.arteksoftware.com/That-Conference/2012/sessions/MonoTouch/links
    - Returns links

GET http://conferences.arteksoftware.com/That-Conference/2012/sessions/MonoTouch/resources
    - Returns resources

GET http://conferences.arteksoftware.com/That-Conference/2012/speakers
    - Returns speakers with sessions

GET http://conferences.arteksoftware.com/That-Conference/2012/speakers/rob-gibbens
    - Returns speakers info

GET http://conferences.arteksoftware.com/That-Conference/2012

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