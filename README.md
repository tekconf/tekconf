# About #
TekConf is a conference management solution, focused on technical conferences.  The main components include a quasi-RESTful API, the web front end, and mobile and desktop apps.

[http://tekConf.com](http://tekConf.com "TekConf.com")<br/>
[http://api.tekconf.com](http://api.tekconf.com "TekConf API")

# Requirements for building/hosting #
- Windows
- Visual Studio 2012
- IISExpress
- MongoDB

# Setting up environment for development #
1. Pull code from develop
2. MongoDB must be installed
3. Restore db (coming soon)
4. db.conferences.ensureIndex( { "position" : "2d" } )

# Backlog #
See Trello [https://trello.com/board/tekconf/50747c7b7dfbc8a32f0417b7](https://trello.com/board/tekconf/50747c7b7dfbc8a32f0417b7 "Trello")

# Use Cases #
Attendees

- Attendee can browse conferences
- Attendee can search for conferences
- Attendee can login
- Attendee can save a schedule for a conference
- Attendee is alerted to changes to a conference schedule

Speakers

- Speakers can find conferences with open calls for speakers/presentations
- Speakers can maintain a speaker profile
- Speakers can upload their presentations
- Speakers can submit presentations to conferences

Organizers

- Organizer can create a conference
- Organizer can add sessions
- Organizer can search for speaker presentations
- Organizer can create schedule

