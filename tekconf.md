- Authentication and Authorization
    - This app will consist of three pieces. A .net core web site (maybe react.js?), a .net core api, and Xamarin mobile/native apps.  Auth should work across all of them
    - I don't want to store passwords, etc myself. Delegate to third parties
    - I do want to store a profile (name, email, etc), and be able to distinguish one person from another
    - I want each person to be able to choose one or _multiple_ auth providers and get the same profile.
    - I want a person to be able to add/remove auth providers from their account
    - I want to include the concept of roles or permissions in the system. These roles will be different depending on the data (I might be an attendee of one conference, an organizer of another, and a speaker at both). Each piece (web, mobile, api) should take this into account (can't POST to certain URLs if permissions don't match)
    - I'm ok with using third party systems (IdentityServer4, Azure AD, Auth0, etc)

-- READ DATA

GET https://tekconf.com/conferences
    - Returns list of active, future conferences
GET https://tekconf.com/conferences/codemash/
GET https://tekconf.com/conferences/codemash/2018/
GET https://tekconf.com/conferences/codemash/sessions
GET https://tekconf.com/conferences/codemash/speakers
GET https://tekconf.com/conferences/codemash/favorites
GET https://tekconf.com/conferences/codemash/tracks
GET https://tekconf.com/conferences/codemash/tags
GET https://tekconf.com/conferences/codemash/sponsors
GET https://tekconf.com/conferences/codemash/maps
GET https://tekconf.com/conferences/codemash/maps/rooms
GET https://tekconf.com/conferences/codemash/about
GET https://tekconf.com/conferences/codemash/code-of-conduct

GET https://tekconf.com/conferences/search?query=codemash
GET https://tekconf.com/conferences/search?near=41.423&32.23123

GET https://tekconf.com/speakers
GET https://tekconf.com/speakers/rob-gibbens
GET https://tekconf.com/speakers/rob-gibbens/profile
GET https://tekconf.com/speakers/rob-gibbens/profile/versions
GET https://tekconf.com/speakers/rob-gibbens/profile/versions/codemash-2018
GET https://tekconf.com/speakers/rob-gibbens/conferences
GET https://tekconf.com/speakers/rob-gibbens/conferences/recommendations
GET https://tekconf.com/speakers/rob-gibbens/presentations
GET https://tekconf.com/speakers/rob-gibbens/presentations/mvvm-with-xamarin
GET https://tekconf.com/speakers/rob-gibbens/presentations/mvvm-with-xamarin/versions
GET https://tekconf.com/speakers/rob-gibbens/presentations/mvvm-with-xamarin/versions/codemash-2018
GET https://tekconf.com/speakers/open-calls

-- CREATE DATA

POST https://tekconf.com/speakers/rob-gibbens/presentations
POST https://tekconf.com/speakers/rob-gibbens/presentations/mvvm-with-xamarin/versions