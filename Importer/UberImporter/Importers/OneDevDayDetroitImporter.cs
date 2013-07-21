//using System;
//using System.Collections.Generic;
//using System.Linq;
//using FluentMongo.Linq;
//using TekConf.Common.Entities;

//namespace UberImporter.Importers.OneDevDay2012
//{
//		public class OneDevDayDetroitImporter
//		{

//				public void Import()
//				{

//						var connection = new MongoDbConnection();
//						var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
//						if (!collection.AsQueryable().Any(c => c.slug == "1DevDayDetroit-2012"))
//						{
//								var conference = new ConferenceEntity()
//																		 {
//																				 //_id = Guid.NewGuid(),
//																				 description =
//																						 @"The DetroitDevDays mission is to build a software developer community in the Detroit area that is regarded as the best in the world.  DevDays educate and unite our Software Developer community with inclusive, accessible and affordable events and conferences. The events are typically held on Saturdays so we do not conflict with attendee work schedules. The cost to attend is kept as low as possible, so developers of all pay scales can afford a ticket.",
//																				 end = new DateTime(2012, 11, 17),
//																				 facebookUrl = "",
//																				 homepageUrl = "http://1devdaydetroit.com/",
//																				 imageUrl = "/img/conferences/OneDevDayDetroit.png",
//																				 lanyrdUrl = "",
//																				 location = "Detroit, MI",
//																				 meetupUrl = "",
//																				 name = "1DevDayDetroit",
//																				 //sessions = new List<SessionEntity>(),
//																				 //slug = "1DevDayDetroit-2012",
//																				 start = new DateTime(2012, 11, 17),
//																				 tagLine = "",
//																				 twitterHashTag = "#1devDayDetroit",
//																				 twitterName = "@devdaydet"
//																		 };

//								conference.AddSession(AddSessionOne());
//								conference.AddSession(AddSessionTwo());
//								conference.AddSession(AddSessionThree());
//								conference.AddSession(AddSessionFour());
//								conference.AddSession(AddSessionFive());
//								conference.AddSession(AddSessionSix());
//								conference.AddSession(AddSessionSeven());
//								conference.AddSession(AddSessionEight());
//								conference.AddSession(AddSessionNine());
//								conference.AddSession(AddSessionTen());
//								conference.AddSession(AddSessionEleven());
//								conference.AddSession(AddSessionTwelve());
//								conference.AddSession(AddSessionThirteen());
//								conference.AddSession(AddSessionFourteen());
//								conference.AddSession(AddSessionFifteen());
//								conference.AddSession(AddSessionSixteen());
//								conference.AddSession(AddSessionSeventeen());
//								conference.AddSession(AddSessionEighteen());
//								conference.AddSession(AddSessionNineteen());
//								conference.AddSession(AddSessionTwenty());
//								conference.AddSession(AddSessionTwentyOne());
//								conference.AddSession(AddSessionTwentyTwo());
//								conference.AddSession(AddSessionTwentyThree());
//								conference.AddSession(AddSessionTwentyFour());
//								conference.AddSession(AddSessionTwentyFive());
//								conference.AddSession(AddSessionTwentySix());
//								conference.AddSession(AddSessionTwentySeven());
//								conference.AddSession(AddSessionTwentyEight());
//								conference.AddSession(AddSessionTwentyNine());
//								conference.AddSession(AddSessionThirty());


//								conference.Save(collection);
//						}
//				}

//				private static SessionEntity AddSessionOne()
//				{
//						var title = @"Going Mobile with Windows Azure Mobile Services";

//						var sessionEntity = new SessionEntity()
//																		{
//																				_id = Guid.NewGuid(),
//																				title = title,
//																				description =
//																						@"When you start building the next great mobile app, do you want to worry about your backend? Do you want to figure out hosting and support servers, or just work on your killer app? Windows Azure Mobile Services allows you to do that. In this session we’ll cover how you can connect iOS and Android applications to a reliable cloud based backend that gives you super easy data storage, user authentication, and push notifications with just a few clicks. You will leave knowing how to get your backend setup so you can store your apps data and more so you can start building apps immediately.",
//																				start = new DateTime(2012, 11, 17, 08, 00, 00),
//																				end = new DateTime(2012, 11, 17, 09, 00, 00),
//																				room = "",
//																				slug = title.ToLower().GenerateSlug(),
//																				twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//																				difficulty = "",
//																				links = null,
//																				prerequisites = null,
//																				resources = null,
//																				sessionType = null,
//																				subjects = new List<string>() { },
//																				tags = new List<string>() { },
//																				speakers = new List<SpeakerEntity>(),
//																		};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Chris";
//						var lastName = "Risner";
//						var speakerEntity = new SpeakerEntity()
//																		{
//																				_id = Guid.NewGuid(),
//																				firstName = firstName,
//																				lastName = lastName,
//																				description =
//																						@"Chris Risner is a Windows Azure Technical Evangelist at Microsoft. Chris is focused on using Windows Azure as a backend for iOS and Android clients. Chris has been working with iOS and Android development for the past several years. Before working in mobile development, Chris worked on many large scale enterprise applications in Java and .NET. Chis is a prodigious learner who loves technology of all flavors and has a vast amount of experience in Smart Clients, Asp.Net MVC, C#,, Java, Objective C, Android and iOS. Chris speaks from his many successes in different areas of technology.",
//																				bitbucketUrl = "",
//																				blogUrl = "",
//																				codeplexUrl = null,
//																				coderWallUrl = null,
//																				company = "",
//																				emailAddress = null,
//																				facebookUrl = null,
//																				githubUrl = null,
//																				googlePlusUrl = null,
//																				isFeatured = false,
//																				linkedInUrl = null,
//																				phoneNumber = null,
//																				profileImageUrl = null,
//																				slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//																				stackoverflowUrl = null,
//																				twitterName = null,
//																				vimeoUrl = null,
//																				youtubeUrl = null,
//																		};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwo()
//				{
//						var title = @"Arduino Hacks";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										@"Arduino is an open-source electronics prototyping platform based on flexible, easy-to-use hardware and software. It’s intended for artists, designers, hobbyists, and anyone interested in creating interactive objects or environments.  Terry May recently began hacking with Arduino and will share his findings and the possibilities of the Arduino world with his usual fun, down-to-earth, curious and un-pretentious flair.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Terry";
//						var lastName = "May";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"Terry May is an infamous Detroit area Software Developer, autodidact and hacker.  He has done everything from small appliance repairman, television production assistant, WILX Graphic Designer, IT guy, assistant video editor, free lance 3D animator, WKBD Graphic Designer, flash guy and Multimedia Specialist.  He started hacking as a teenager on Commodore 64s and CB radios. Terry is a polyglot programmer and ECMAScript virtuoso.  Today he works on Android Apps, Unity 3D and Augmented Reality projects for Detroit Labs.  Terry recently began hacking with Arduino and will share his findings and the possibilities of the Arduino world with his usual fun, down-to-earth, curious and un-pretentious flair.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionThree()
//				{
//						var title = @"Asynchronous web programming on the JVM";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Waiting is over. Welcome to the world of asynchronous web programming on JVM. Why non blocking web programming is important? Because every time you wait for something to happen a kitten dies. Seriously because it will help your web application to handle more request with the\r\nresources you have already. Non blocking techniques are already used by high traffic websites like facebook and linkedin. But you don’t have to be facebook or linkedin to take the benefits non blocking web programming. In this presentation I will explain the theory behind the non blocking (also known as asynchronously) web programming and the tools available to you to build scalable web applications in JVM.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Nilanjan";
//						var lastName = "Raychaudhuri";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"Nilanjan is a consultant and trainer for Typesafe. He has more than 12 years of experience managing and developing software solutions in Java/JEE, Ruby, Groovy and also in Scala. He is zealous about programming in Scala ever since he got introduced to this beautiful language. He enjoys sharing his experience via talks in various conferences and he is also the author of the “Scala in Action” book.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionFour()
//				{
//						var title = @"Continuous Integration in the Mobile World";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										@"Just because you’re coding Mobile applications doesn’t mean that you can’t take advantage of the benefits Continuous Integration (CI). Come to this session and learn all about CI on both the iPhone and Android platforms. Learn the difference between Hudson and Jenkins, all about headless emulators, as well as the best tools to use for unit testing, functional testing and beta app deployment of your mobile apps.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Godfrey";
//						var lastName = "Nolan";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"Godfrey Nolan is founder and president of RIIS, a mobile development firm in the Detroit Metro area. He is also author of “Decompiling Java” and “Decompiling Android.” Godfrey has spoken at JavaOne, ASP-Connections, VSLive, Codemash, Code PaLOUsa, 1DevDay, and many local Java and .NET user groups on a wide range of topics, such as continuous integration, executable requirements and mobile security.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionFive()
//				{
//						var title = @"Design Patterns and Backbone in JavaScript";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"For the presentation I will create a monster themed Web Application using MV* architecture and Backbone.js through the presentation and application I will explain how JavaScript benefits from Design Patterns to prevent ‘monstrous code’.\r\n\r\nAn alarming amount of web developers have adopted JavaScript by tying their data directly to the DOM. When web applications use a lot of javascript, this becomes a hinderance and prevents efficient, maintainable javascript. It becomes a monstrous task keeping the data in sync between the HTML UI, the Javascript Logic, and the database model. For rich client-side applications developers can utilize the same design patterns that they’ve used for years to make their applications more structured and maintainable. Through Design Patterns developers force their applications to become more modular, containing a set of highly decoupled, distinct pieces of functionality. \r\n\r\nThe two main Design Patterns that my presentation will focus on are PubSub and MV*.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Aaron";
//						var lastName = "Maturen";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"Aaron Maturen is a web developer for Saginaw Valley State University with a passion for learning. He has dabbled in many different languages, but is known for being a Python and Javascript connoisseur. You’ll often find him reading the source of fun javascript libraries or the ECMAScript spec. Aaron has also been labeled the “database guy” to many, and has been known to “make databases cry”. Aaron is also a developer for a small consulting firm, Ivory Penguin, specializing in education software, digital asset management, and the mobile web.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionSix()
//				{
//						var title = @"Effective Code Quality through Behavior-Driven Development (BDD)";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Extreme Programming originally had the rule to “test everything that could possibly break”. This practice has evolved into what is known as Test Driven Development (TDD). But TDD tooling still forces developers to think in terms of tests and assertions instead of specifications, requirements, and end-user value.\r\n\r\nBehavior Driven Development (BDD) changes the game by shifting the focus to usage interactions of software.  BDD was developed by the Dan North as a response to the issues encountered in teaching Test DrivenDevelopment.  It combines using native language with the ubiquitous language of Domain Driven Design to describe the purpose and capability of the code being written.  This separation of concerns allows the domain experts to define the expected behavior of the system which is translated into test cases even before the software itself is written.   The use of a language close to the human language increases the early and often collaboration between the Business Analyst and the Developers.\r\n\r\nThis session covers the concepts and value of adopting BDD and Integration testing into your softwaredevelopment practice and the frameworks available (JBehave, Cucumber, SpecFlow).  It then jumps into demonstrating the testing framework in action with practical examples of usage.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Murali";
//						var lastName = "Mogalayapalli";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Murali Mogalayapalli is presently Senior Software Architect at New World Systems in Troy for the Police and Fire Dispatch/Mobile solution. His 20 years of industry experience spans a variety of segments, such as Public Safety, Application Performance Monitoring and warehouse management systems. Murali has applied a breadth of technologies and stacks (Java, .NET, and various open source) over his career, and is currently focusing on performance and scale for high-availability production systems.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionSeven()
//				{
//						var title = @"Functional Javascript";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"The focus of this talk is to explore functional programming paradigms and how they can be applied in Javascript. Functional programming paradigms can help us: write smaller units of code which will enable more thorough unit testing – so that code breaks less often – and reuse those smaller blocks of code in more ways – so that we need to write less code. Slides will present some core concepts to functional programming, their definition and application in Javascript, followed by actual code refactoring.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Joshua";
//						var lastName = "Kalis";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"I am a UI Engineer at Quicken Loans and Javascript fanatic. My interest in being a polyglot programmer has influenced the way I approach writing code; hopefully for the better.I have pulled OO ideas from Java and C# as well as Functional concepts from F#, Scheme, Erlang, and Haskell. Mostly my learning of and from languages, other than Javascript, has helped me write better Javascript since I have very few options in the browser. I think that learning new languages is me just looking for a better way to do what I love to do.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionEight()
//				{
//						var title = @"Functional Principles for OO Development";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"How is an expert OO developer to improve on his craft? By learning from other paradigms! Six principles of the functional style can apply to OO. Some of these principles are part of good practice already; some express patterns both old and new; all give us different ways of thinking about problems. For developers with no expertise in functional programming, examples in C#, Java, F# and Scala show new techniques for writing clear, quality code.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Jessica";
//						var lastName = "Kerr";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Jessica Kerr is a long-time Java developer turned polyglot, engaged in writing Scala for biotech. She loves speaking at St. Louis user groups and at conferences like CodeMash, DevLINK, and DevTeach — but her #1goal is keeping two young daughters alive without squelching theirinner craziness. Find her thoughts at blog.jessitron.com and @jessitron.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionNine()
//				{
//						var title = @"Graph Traversals in Neo4j with Gremlin Java";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Graph databases are a NoSQL / polyglot persistence solution that provide a natural way to model and compute on complex, interrelated data. We’ll use Gremlin’s Groovy and Java dialects to see traversals in action on a network data set in Neo4j and Titan, a distributed graph database optimized for Big Graph Data.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Bobby";
//						var lastName = "Norton";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Bobby Norton is the co-founder of Tested Minds, a startup focused on products for social learning and rapid feedback. He has built software for over ten years at firms such as Lockheed Martin, NASA, GE Global Research, ThoughtWorks, DRW Trading Group, and Aurelius. His tools of choice include Java, Clojure, Ruby, Bash, and R. Bobby holds a M.S. in Computer Science from FSU.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTen()
//				{
//						var title = @"Iconoclasm – Keynote";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"History is littered with the stories of iconoclasts–people who truly stood out as pioneers, lateral thinkers, and in some cases, outright heroes–and their successes and failures. From the baseball management vision of Branch Hickey to the glassblowing vision of Dale Chihuly to the engineering design vision of Steve Jobs, iconoclasts have changed our world in subtle and profound ways, sometimes loudly, sometimes quietly. For an industry that seems so ripe and so rife with “special personalities”, it would seem that programming is tied up deeply with iconoclasm. But what defines the iconoclast, what demarcates the “true” iconoclast from the mere pretender, and how can we use the characteristics of the iconoclast to change our own immediate surroundings for the better?",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Ted";
//						var lastName = "Neward";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Ted Neward is an independent consultant specializing in high-scale enterprise systems, working with clients ranging in size from Fortune 500 corporations to small 10-person shops. He is an authority in Java and .NET technologies, particularly in the areas of Java/.NET integration (both in-process and via integration tools like Web services), back-end enterprise software systems, and virtual machine/execution engine plumbing.\r\n\r\nHe is the author or co-author of several books, including Effective Enterprise Java, C# In a Nutshell, SSCLI Essentials, Server-Based Java Programming, and a contributor to several technology journals. Ted is also a Microsoft MVP Architect, BEA Technical Director, INETA speaker, former DevelopMentor instructor, frequent worldwide conference speaker, and a member of various Java JSRs. He lives in the Pacific Northwest with his wife, two sons, and eight PCs.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionEleven()
//				{
//						var title = @"Intro to iOS Development";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Development for mobile platforms has gotten easier as the languages and tools have matured.  In this presentation, Brian will introduce development for iOS and demonstrate how easy it can be by building a simple application from scratch using the native tools freely available from Apple.  This hour will include:\r\nAn introduction to Xcode and the iOS Simulator\r\nBuilding user interfaces and transitions using Storyboards\r\nUsing the Cocoa Touch Model View Controller framework\r\nLoading data into model objects and passing it between views\r\nA discussion of performance considerations when developing for a mobile platform",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Brian";
//						var lastName = "Munzenberger";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"Brian has 8 year" + @"s of experience in all aspects of the software development lifecycle. Past projects include B2B, B2C, and large-scale e-commerce applications. Throughout his career Brian has worked with a wide range of companies from small startups to large corporations. Brian brings his passion for development and knowledge of large-scale applications to the world of iOS.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwelve()
//				{
//						var title = @"Is a Mobile-Friendly Website Enough?";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Do I Really Need a Mobile App, or is a Mobile-Friendly Website Enough?\r\nMobile apps are the hottest trend in tech right now, leaving many companies struggling to create a mobile strategy for their products or services. When defining this strategy, the question of “do I really need a mobile app” is the first barrier to be cross. Learn more about how to explain to a company why they truly need a custom mobile application, and when updating a portion of an existing web site, using responsive techniques, to be mobile device-friendly may be the more appropriate solution. (For any audience, recommended as a keynote presentation.)",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Jeff";
//						var lastName = "McWherter";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Jeff McWherter is a Partner and the Director of Development at Gravity Works Design and Development. Jeff is a graduate of Michigan State University and has over 16 years of professional software development experience. In 2012 Jeff published his third book Professional Mobile Development (Wrox Presss) which complements his other works Testing ASP.NET Web Applications (Wrox Press) and Professional Test Driven Development with C# (Wrox Press).\r\n\r\nJeff is very active in developing programming communities across the country, speaking at conferences and organizing events such as the Lansing Give Camp, pairing developers with non-profit organizations for volunteer projects.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionThirteen()
//				{
//						var title = @"It’s Just a Web Site: How Poor Web Programming is Ruining Information Security";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"A review of recent web site attacks will be given to help understand what major vulnerabilities are common for web sites, how attacks are executed, and what a compromise can mean to a company, government, or other organization. Further attention will be given to: how an entity can prevent poor programming from ruining their security; how web programmerscompare to other industries for qualifications required to interact with highly sensitive data; and a forward-thinking discussion on how the industry can be proactive when hiring programmers. The goal of this presentation is to make all parties involved in information security aware of just how serious one poorly created web site can be to fabric of theirinformation security architecture and practices.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Mark";
//						var lastName = "Stanislav";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Mark Stanislav is a Senior Consultant at NetWorks Group, focused on operational automation and information security. With a career spanning a decade, Mark has worked within small business, academia, start-up, and corporate environments primarily focused on Linux architecture, information security, and web application development. Through the recent years of his career, Mark has had an opportunity to architect and deploy cloud infrastructure within many different industries and for various business needs. Mark holds a Bachelor’s degree in Networking & IT Administration and a Master’s in Technology Studies focused on Information Assurance, both from Eastern Michigan University. Mark also holds his CISSP, Security+, Linux+, and CCSK certifications.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionFourteen()
//				{
//						var title = @"Java on the mainframe – More than what you think!";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"In this presentation I’ll describe the mainframe environments in which Java runs, giving some background on one that is unique to z/OS. I’ll then outline how to develop and execute Java programs on z/OS. I’ll demonstrate, showing lots of code, how to connect Java to traditional mainframe programs. That’s the hard part! I’ll follow-up with some interesting Java and z/OS factoids. The mainframe operating system has many components and we will encounter a number of them during this presentation. I’ll provide a high-level description of those components and of z/OS itself. By the end of the session, you’ll have some appreciation for z/OS. And, just maybe, you’ll consider z/OS as a target platform for your own Java applications.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Walter";
//						var lastName = "Falby";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Walter Falby has more than 30 years of experience in application development, operating system and subsystem enhancements and product development. He has worked on MVS, Windows, OS/2, UNIX and Linux. The programming languages he has used include assembler, C/C++, C# and Java. He has published books on programming and alternate energy research as well as articles on software development and bacteriological studies of recreational lakes.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionFifteen()
//				{
//						var title = @"Magic Bullet, Thy Name Is Responsive Design";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"The session will cover a brief history of responsive design (it actually goes back a ways), Responsive Design vs. Adaptive Design, why you might or might not need a separate mobile site, “Mobile First” design philosophy and how that integrates with a successful responsive website, pitfalls of designing a responsive site, load time optimization and how that might affect your choice of javaScript libraries and CSS3 bits, how IE8 fits in to all of this, and  then some “check this out” stuff with links to sites.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Marc";
//						var lastName = "Nischan";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionSixteen()
//				{
//						var title = @"MapReduce and Its Discontents";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Apache Hadoop is the current darling of the “Big Data” world. At its core is the MapReduce computing model for decomposing large data-analysis jobs into smaller tasks and distributing those tasks around a cluster. MapReduce itself was pioneered at Google for indexing the Web and other computations over massive data sets.\r\n\r\nI’ll describe MapReduce and discuss its strengths, such as cost-effective scalability, as well as its weaknesses, such as the latencies that limit its usefulness for real-time event stream processing and the relative difficulty of writing MapReduce programs. I’ll briefly show you how higher-level languages ease the development burden and provide useful abstractions for the developer.\r\n\r\nThen I’ll discuss emerging alternatives, such as Google’s Pregel system for graph processing and event stream processing systems like Storm. I’ll emphasize whyfunctional programming is so important for big data applications. Finally, I’ll speculate about the future of Big Data technology.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Dean";
//						var lastName = "Wampler";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Dean Wampler is a Principal Consultant for Think Big Analytics, specialists in “Big Data” application development, primarily using Hadoop-related technologies. He wrote “Functional Programming for Java Developers” and co-wrote “Programming Scala” and “Programming Hive” (all from O’Reilly). Dean is the founder of the Chicago-Area Scala Enthusiasts (meetup.com/chicagoscala/) and the programming web site polyglotprogramming.com. He is also a contributer to several open-source projects. For mindless tweets, see @deanwampler.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionSeventeen()
//				{
//						var title = @"Oh NODE you didn’t!";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Let’s walk through building an application using Node. I will demonstrate building a Node.js application and dive into some of the key components that allow Node to be event-driven and non-blocking by leveraging Socket I/O and Node’s EventEmitter.\r\n\r\nNode is designed for writing highly scalable Internet applications, notably web servers. Programs are written in JavaScript, using event-driven, asynchronous I/O to minimize overhead and maximize scalability.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Calvin";
//						var lastName = "Bushor";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Calvin Bushor is a JavaScript fanatic and full time tinkerer pushing the boundaries of the web on a daily basis. As a Senior Software Engineer at Quicken Loans, Calvin leads the technical development of MyQL.com’s expansive user experience with the explicit goal of making the mortgage process engaging and easy to understand. When Calvin is not writing bulletproof Javascript apps or experimenting with new web technologies, you can find him at home with his wife Katie or out playing soccer.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionEighteen()
//				{
//						var title = @"OpenGL";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"OpenGL is a standard specification defining a cross-language, multi-platform API for writing applications and simulating physics, that produce 2D and 3D computer graphics. The interface consists of over 250 different function calls which can be used to draw complex three-dimensional scenes from simple primitives.\r\n\r\nWe will cover the need to know features of OpenGL ES 2.x, the standard for 3D graphics and the upcoming standard for WebGL, bringing 3D to desktop browsers. Attendees will come away with an understanding of what is needed to develop 3D apps for OpenGL ES.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Bob";
//						var lastName = "Kuehne";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Before starting Ann Arbor’s Blue Newt Software, Bob Kuehne was the Technical Lead for the OpenGL Shading Language at Silicon Graphics. Bob Kuehne has worked for more than a decade in the computer graphics industry, working his way up and down the OpenGL food chain, from writing OpenGL code to writing shader compilers. He has presented on OpenGL at numerous conferences, including SIGGRAPH.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionNineteen()
//				{
//						var title = @"Regular expressions – now you have two problems";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Be afraid. Be very afraid. For you are about to enter the mysterious and forboding land of regular expressions. A land of strange-looking hieroglyphics. A land with many flavors. A land where its devotees possess seemingly magical powers over text. But be afraid no more, for you have a guide. And your guide will show you the way through this land.\r\nIn this session, you’ll learn the basics of how a regular expression engine works. Combining this knowledge with the dozen or so regex operations, you’ll be able to craft your own regular expressions with confidence. Perhaps more importantly, you’ll be able to decipher exactly what an existing regular expression does. In the end, you’ll probably discover that regular expressions are not as scary as you once thought.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Brian";
//						var lastName = "Friesen";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"XXX",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwenty()
//				{
//						var title = @"Reverse Engineering .NET and Java";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										@"Learn the various techniques bad guys can use to extract information from your .NET or Java applications or at least how you can recover the source code that your predecessor deleted before he quit. A demo filled session on how easy it is to extract information from virtually any .NET or Java application.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Joe";
//						var lastName = "Kuemerle";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Joe Kuemerle is a developer and speaker in the Cleveland, OH area specializing in .NET development, security, database and application lifecycle topics. He is currently a Lead Developer at BookingBuilder Technologies. Joe is active in the technical community as well as aspeaker at local, regional and national events. Joe blogs at www.kuemerle.com and is on Twitter as @jkuemerle.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentyOne()
//				{
//						var title = @"Rise of the Web App";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"The web started as a platform for documentation and gained a good deal of interactivity during the last decade. Now, the web is moving forward as an application platform in ways that no one would have predicted a few years ago. I’ll talk about some of the technology, tools and services that are raising the web up to the next level. Web standards like media queries, appcache, IndexedDB and WebGL provide capabilities for a new set of applications, and Mozilla’s WebAPI project enables an entire phone to be built of the web (FIrefox OS/Boot2Gecko).",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Kevin";
//						var lastName = "Dangoor";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Kevin Dangoor is product manager for Mozilla’s developer tools. Though he’s worked with many languages in many environments, he is best known for his Python work as the founder of theTurboGears web framework and Paver project scripting tool. He has spoken at numerous conferences and is a co-author of Rapid Web Applications with TurboGears. More recently, his work at Mozilla has involved the Bespin browser-based code editor, starting the CommonJS project, and a new generation of developer tools for Firefox. He lives in Ann Arbor, Michigan.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentyTwo()
//				{
//						var title = @"Scala: Objects and Methods and Functions, Oh My!";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										@"In Scala, every value is an object. Even functions are objects. So what does that mean for you? Join us for an introductory-level discussion of this functional-object oriented language. Regardless of your programming language background, you will be able to write simple programs after one hour. If you choose not to run screaming from the room, you will be prepared to dive deeper into Scala!",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Dianne";
//						var lastName = "Marsh";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										@"XXX",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentyThree()
//				{
//						var title = @"Self Engineering – Keynote";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"How do you apply engineering discipline to the thing that matters most: yourself? We’re software engineers or systems engineers or network engineers. We have learned lessons the hard way. These lessons boil down to immutable laws; some things work and some things don’t.\r\nSuccessful engineers apply these hard-won lessons to their professions. We use the same techniques day in and day out to create systems to make other people’s lives better. But few of us apply the same kind of systems thinking to our own lives, careers, jobs, or organizations.\r\nLet’s start thinking of our daily lives and work as a system, just like a system we would create or maintain in our professions. How would you approach your life differently if it were a system you were creating for a client? How do you define success? How do you measure it? How do internal and external quality differ?",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Chad";
//						var lastName = "Fowler";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Chad Fowler is an internationally known software developer, trainer, manager, speaker, and musician. Over the past decade he has worked with some of the world’s largest companies and most admired software developers.    Chad is SVP of Technology at LivingSocial. He is co-organizer of RubyConf and RailsConf and author or co-author of a number of popular software books, including Rails Recipes and The Passionate Programmer: Creating a Remarkable Career in Software Development.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentyFour()
//				{
//						var title = @"Testing with Spock";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Spock is a testing and specification framework for Java and Groovy applications. What makes it stand out from the crowd is its beautiful and highly expressive specification language. Thanks to its JUnit runner, Spock is compatible with most IDEs, build tools, and continuous integration servers. Spock is inspired from JUnit, jMock, RSpec, Groovy, Scala, Vulcans, and other fascinating life forms.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Elizabeth";
//						var lastName = "Henderson";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Elizabeth Henderson has 9 years of experience in software development in the insurance and in the higher-education fields. In her professional career, she has developed in projects including single sign-on implementations, mobile applications, tutoring systems, and insurance quoting systems. Although most of her experience is in Java, she loves experimenting with different JVM languages, jQuery, and iOS. Elizabeth has spoken at the Lansing Java Users Group in addition to many professional presentations. She tweets at @elizhender.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentyFive()
//				{
//						var title = @"The class that knew too much: refactoring spaghetti code";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"The Single Responsibility Principle states that a class should have one (and only one) reason to change. Classes that don’t adhere to this principle can result in tightly coupled spaghetti code that leads to more bugs and higher maintenance. We’ll look at how to identify these classes, and how to clean them up using refactoring tools, design patterns, and even aspect-oriented programming.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Matt";
//						var lastName = "Groves";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Matthew D. Groves is a guy who loves to code. It doesn’t matter if it’s “enterprisey” C# apps, cool jQuery stuff, contributing to OSS, or rolling up his sleeves to dig into some PHP. He has been coding professionally ever since he wrote a QuickBASIC point-of-sale app for his parent’s pizza shop back in the 90s. He currently works from home on the Telligent product team, and loves spending time with his wife and 2 kids, watching the Cincinnati Reds, and getting involved in the developer community. He is currently writing a book for Manning about aspect-oriented programming in .NET, and also teaches a class on web development at Capital University in Columbus, Ohio.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentySix()
//				{
//						var title = @"The State of F#- Why You Should (or Shouldn’t) Care";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"In this talk, we’ll discuss the past, present, and future of F# programming. You’ll learn about the latest F# innovations like Type Providers, but more importantly, you’ll gain an understanding of how F#’s unique features can help you solve problems for your clients. You’ll also learn about some situations that are not a great fit for F#, and how to avoid them. We’ll wrap up with a discussion about the future of F#. By the end of this talk, you should have a good idea of what F# brings to the table, and how you do (or don’t) want to use it in your organization.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Chris";
//						var lastName = "Marinos";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Chris is a F# MVP and software consultant in Ann Arbor, MI. A proponent of F# since its pre-release days, he has given numerous F# talks and trainings throughout the US and Europe. He has also written articles on F# for MSDN Magazine and his F#-centric blog. His other technical interests and experiences include coffeescript, backbone.js, Rails, Django, C#, and of course, functional programming. When not coding, he enjoys video games, BBQ food, and obnoxiously large TVs.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentySeven()
//				{
//						var title = @"Using types to write your code for you";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"In languages with type inference (Haskell, Scala, F#), the type system can be an invaluable tool that not only can prevent silly errors but can actually make algorithm development easier. In fact, if you’re careful with how you construct your types you can practically make the types write the code for you! In this talk I’ll show how moving some information about the structures in your code into the type system can convert some of the hard work of algorithm development into something more akin to assembling puzzle pieces, and I’ll give some pointers on how to do that. I’ll show how you can construct your data types so that meaningless, “impossible”, or other error conditions cannot even be represented in your structures and the awesome effects that can have on your code. I’ll walk through a couple examples to illustrate the techniques.\r\nMy examples will be in Haskell but the ideas and techniques should apply to any language with Hindley-Milner type inference, especially those with constructs similar to Haskells’ type classes.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Job";
//						var lastName = "Vranish";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"By the time I began college I was already fairly proficient at software development and wanted to try something new so I decided to study electrical engineering. My undergraduate degree is a BS in Electrical Engineering from Calvin College. Having skills in both the hardware and software realms makes me particularly suited to embedded software development which often requires forays into the hardware side of things.\r\n\r\nAfter college I worked at GE Aviation developing safety critical embedded software for aircraft flight management systems. I became interested in test driven development and agile methods while at GE and when I could not make these things happen there I moved to Atomic Embedded (a pioneer in applying Agile methods to embedded software development) in June 2011.\r\n\r\nMost of my free time is now taken up with entertaining and bouncing a small baby named Jasper, but otherwise, I enjoy Haskell (and dream of one day being able to write embedded software in a functional language), the theory and implementation of programming languages, vegetarian food and racquetball.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentyEight()
//				{
//						var title = @"vert.x";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"Vert.x is an event driven application framework that runs on the JVM – a run-time with real concurrency and unrivalled performance. Vert.x then exposes the API in Ruby, Java, Groovy and JavaScript. So you choose what language you want to use. (Scala, Clojure and Python support is on the roadmap too).\r\n\r\nVert.x also bundles a host of goodies out-of-the-box including a distributed event bus, Web Sockets, SockJS, a MongoDB persistor and many other features so you can write real applications from the set-off.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Mac";
//						var lastName = "Liaw";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Mac Liaw entered The Ohio State University’s Masters Program in Computer Science at age 15. He was a member of the CERN development team that established HTTP and HTML. He is active in the Linux Kernel, Groovy development and Haskell open source project. Currently he is CylaTech.com Inc’s VP of Technology and has been involved in video games development since Playstation 2 and Xbox. He currently oversees PS3 and 360 video games development and special effects projects at CylaTech.com Inc. He also serves as the CTO of Bringshare,  a startup launched its public beta in fall 2011 at the DEMO conference in Silicon Valley. Bringshare is an online tool for marketing professionals, entrepreneurs and their businesses measure and evaluate their online marketing investments more efficiently and cost effectively.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionTwentyNine()
//				{
//						var title = @"Where is your domain model?";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										"A domain model is much more than the “M” in MVC. As Martin Fowler writes, “Learning how to design and use a Domain Model is a significant exercise–one that has led to many articles on the ‘paradigm shift’ of objects use.”\r\n\r\nLet’s talk about evolving a domain model in sustainable ways that allow a codebase to organically grow in a way that supports agility and adaptability. Highlights include:\r\n\r\n* What is a domain model? What are the alternatives?\r\n* What is sustainable software development?\r\n* Ubiquitous language, and thinking twice about naming something a Manager, Service, or DAO\r\n* Sharing objects between client and server?\r\n* Why mock objects?\r\n* Bringing it together with outside-in test-driven development\r\n\r\nExamples are in Ruby, JavaScript, and Java, but this is not a language-specific session.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Baraa";
//						var lastName = "Basata";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"I am a consultant with Pillar, making things happen on client engagements for agile project delivery. On every project, my focus is on how to best contribute to the success of the teams I serve, and I’m constantly looking for every opportunity to make a positive impact and to delight my clients. I studied Mathematics and Computer Science at the University of Michigan and Lawrence Technological University, and I reside in Flint, Michigan. Follow me on Twitter @baraabasata.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

//				private static SessionEntity AddSessionThirty()
//				{
//						var title = @"Your Asynchronous Future";

//						var sessionEntity = new SessionEntity()
//						{
//								_id = Guid.NewGuid(),
//								title = title,
//								description =
//										".NET 4.5 and C# 5 introduce new idioms for asynchronous programming. We’ve had the Asynchronous Programming Model (BeginDoSomething, EndDoSomething and IAsyncResult). We’ve had the Event Based Asynchronous Pattern (OnDoSomethingCompleted), and the Task based library in .NET 4. The latest update to the language and libraries provides language support for asynchronous programming, enabling you to concentrate on you core algorithms instead of the plumbing of asynchronous programming models. It makes asynchronous programming easier, but not necessarily easy.\r\n\r\nIn this session, we’ll go over the most common practices for asynchronous programming, and a number of techniques that will help you create more correct and more maintainable asynchronous programs. We’ll also briefly discuss how these techniques translate to WinRT and Metro.",
//								start = new DateTime(2012, 11, 17, 08, 00, 00),
//								end = new DateTime(2012, 11, 17, 09, 00, 00),
//								room = "",
//								slug = title.ToLower().GenerateSlug(),
//								twitterHashTag = "#rc-" + title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//								difficulty = "",
//								links = null,
//								prerequisites = null,
//								resources = null,
//								sessionType = null,
//								subjects = new List<string>() { },
//								tags = new List<string>() { },
//								speakers = new List<SpeakerEntity>(),
//						};

//						var twitterName = "";
//						if (!string.IsNullOrWhiteSpace(twitterName) && !twitterName.StartsWith("@"))
//						{
//								twitterName = "@" + twitterName;
//						}
//						var firstName = "Bill";
//						var lastName = "Wagner";
//						var speakerEntity = new SpeakerEntity()
//						{
//								_id = Guid.NewGuid(),
//								firstName = firstName,
//								lastName = lastName,
//								description =
//										"Bill Wagner has spent most of his professional career between curly braces, starting with C and moving through C++, Java, and now C#. He’s the author of Effective C# (2nd edition released in 2010), More Effective C# (2009), and is one of the annotators for The C# Language Specification, 3rd and 4th editions. He is a regular contributor to the C# Dev Center, and tries to write production code whenever he can. With more than 20 years experience, Bill Wagner, SRT Solutions co-founder and CEO, is a recognized expert in software design and engineering, specializing in C#, .NET and the Azure platform. He serves as Michigan’s Regional Director for Microsoft and is a multi-year winner of Microsoft’s MVP award. An internationally recognized author, Bill has published three books on C# and currently writes a column on the Microsoft C# Developer Center. Bill was awarded the Emerging Technology Leader Award by Automation Alley, Michigan’s largest technology consortium. Bill earned a Bachelor of Science degree in computer science from the University of Illinois at Champaign-Urbana. Bill blogs at http://www.srtsolutions.com/billwagner and tweets athttps://twitter.com/billwagner.",
//								bitbucketUrl = "",
//								blogUrl = "",
//								codeplexUrl = null,
//								coderWallUrl = null,
//								company = "",
//								emailAddress = null,
//								facebookUrl = null,
//								githubUrl = null,
//								googlePlusUrl = null,
//								isFeatured = false,
//								linkedInUrl = null,
//								phoneNumber = null,
//								profileImageUrl = null,
//								slug = (firstName.ToLower() + " " + lastName.ToLower()).GenerateSlug(),
//								stackoverflowUrl = null,
//								twitterName = null,
//								vimeoUrl = null,
//								youtubeUrl = null,
//						};

//						sessionEntity.speakers.Add(speakerEntity);
//						return sessionEntity;
//				}

      




//		}


//}

