# Cyber Security Awareness Chat Bot

# overview Part 2
This project is a WPF desktop application that provides an interactive Cyber Security chatbot to educate users about cybersecurity topics such as password safety, scam detection, privacy protection, and phishing awareness. The bot engages users in a conversational manner, remembers their name and favorite topics, and responds with detailed tips, empathy, and follow-up suggestions.

# Overview Part 3
The Chatbot fetures have been improved and are more advanced compared to Part 2. the features added are task window which collects,stores and remind the user about their tasks, Quiz game that asks cyber security questions, Activity log that stored the history of user activities and finally the help feature that help navigate the chatbot

# Purpose
The chatbot aims to raise cybersecurity awareness in an engaging, low‑pressure way. By varying responses, remembering user preferences, and showing empathy, it encourages users to learn about online safety without feeling overwhelmed. The project demonstrates practical use of WPF, asynchronous UI updates, sentiment analysis (keyword‑based), and conversation state management. This project was developed as part of a cybersecurity learning exercise. It demonstrates how programming can be used to promote awareness and educate users about online safety in an interactive and engaging way. 

# Core Features
- ASCII ART - stands as the logo of the program
- User‑friendly chat interface with rounded borders, a custom colour scheme (aquamarine/dark blue), and a typing‑animation effect.
- Greeting sound (Welcome.wav) plays when the application starts.
- Name collection – the bot asks for the user’s name and personalises future responses.
- Keyword recognition – detects words like “password”, “scam”, “privacy”, “phishing” and returns detailed, pre‑written cybersecurity advice.
- Random tip system – provides a variety of tips on each topic so the user gets different information on repeated requests.
- Sentiment detection – identifies emotions (worried, curious, frustrated) and responds empathetically before offering helpful tips.
- Follow‑up handling – if the user says “tell me more” or “another tip”, the bot gives a different random tip about the last discussed topic.
- Favourite topic memory – the bot remembers when a user says they are interested in a specific topic.
- Exit command – typing “exit”, “quit”, or “goodbye” closes the application politely.

# Recent core features
- task window - setting, getting, deleting and completing tasks
- quiz window - game with questions
- clapping wav sound - cheers the user for completing the quiz
- Activity log - stores history of activitiies
- help - help navigate the chatbot

 # Architecture And Key Components
 - MainWindow.xaml.cs = Orchestrates the entire conversation flow: validates input, calls the appropriate manager classes, updates the UI with typing animations, and shows temporary “Thinking…” messages.
 - CyberSecurityChatBot = Stores long, comprehensive answers for the main topics (used when a keyword is detected without an explicit “tip” request).
 - RandomResponseManager =  Holds multiple shorter tips per topic and returns a random one on demand.
 - ConversationManager = Tracks the current topic and detects follow‑up phrases.
 - SentimentDetector = Scans input for emotional keywords and returns an empathetic response.
 - Chatbot = Manages user‑specific data (name and favourite topic).
 - TypingStyle = Provides the character‑by‑character typing effect for messages.
 - AudioPlayer = Plays the greeting sound (handles exceptions gracefully).

# Recent Architecture And Key Components
- TaskWindow.xaml.cs - this window allows users to add tasks, delete tasks, mark completed tasks as complete, and refresh the list of tasks. The user can also set a date for a reminder and the system will keep track and remind the user of their due task. i stores all the tasks and reminders using mysql data storage
- QuizWindow.xaml.cs- This window plays with the user by asking them cyber security questions, keeps track of the progress, calculates the score and a hands clapping sound is played whn the user finishes the quiestions.
- ActivityLogger.cs - which stores all the history of the users actions or activities in the project.
- Help feature that helps the user how to navigate the chatbot.
- FLPProcessing.cs feature that detects the users intends it keyword detection and allowing tasks, quizes, logs on the chat display in the main window
- TaskWindow.xaml - it is responsible for the desin of the task window
- QuizWindow.xaml - it is responsible for the design of the quiz window
- TaskManager.cs - manages all the task functions
- Clapping hands wav sound - when the user completes the quizes the sound plays to congradulate them
- Instant feedback - instant feedback is provided immidietly when the answer is pressed on the quizes

# Conversation Flow
- Start → Play greeting → Display welcome message → Ask for name.
- User enters name → Store it → Show topic options → Ask what they want to learn about.
- User message is processed in this order:
- Exit command? → Goodbye and shut down.
- Contains “tip”? → Detect topic → Return random tip for that topic → Set as current topic.
- Sentiment detected? → Empathy response → Optionally give a tip.
- Follow‑up phrase? → Use ConversationManager to give another random tip on the same topic.
- “Interested in…”? → Save as favourite topic.
- Contains keyword? → Return detailed answer from CyberSecurityChatBot.
- None of the above → Ask user to rephrase.
- After each response → Clear input box → Scroll chat view.

# Recent Added Conversation Flow
- Start → Play greeting → Display welcome message → Ask for name.
- User enters name → Store it → Show topic options → Ask what they want to learn about.
- User message is processed in this order:
- Exit command? → Goodbye and shut down.
- User enters add task
- FLPProcessor check for intent (add task)
- user enter title, description and date for reminder
- task added to database
- Task window - user can delete, mark complete, show list
- Quiz game - asks questions with answer options, user answers.
- Activity logger - shows history
- help - shows key detection, and navigation 

  
# User Interface (XAML)
- A centered border containing the title and a shield‑shaped ASCII art.
- A large rounded RichTextBox for the chat history (read‑only, scrollable).
- A rounded TextBox for user input (supports Enter key to send).
- A dark blue “Send” button aligned to the bottom right.
- Background colour: MediumAquamarine with Aquamarine accents.
  
# Technologies Used
- .NET (WPF) – for the desktop UI framework.
- C# – all logic and asynchronous typing animations (async/await).
- XAML – declarative UI layout and styling.

# Recent Technologies Used
- MyQSL.DATA package
  
# Usage
- click my github link which will take you to my resipoties then you click the CyberSecurityChatBot-Poe option.
- click the green button that is written code and options will be provided and you shold choose the compressed zip file and download it
- open the microsoft visual studio and open the downloaded zip solution
- Run the application in Visual Studio.
- Enter your name when prompted.
- Ask questions related to cybersecurity topics provided by the bot.
- Type "exit" or "Goodbye" to end the session.

# Recent Usage
- can add task via text area
- intialise quiz via text area
- play quiz game on the quiz window
- add task
- see you history
- get help with navigation

  # Future improvements
  - Add more topics
  - add more advanced features
  - Store user interaction history

 # References
 -  DeepSeek-AI (2025). *DeepSeek-R1: Incentivizing Reasoning Capability in LLMs via Reinforcement Learning*. Available at: https://arxiv.org/abs/2501.12948 (Accessed: [20/05/2026]).
 -  IAmTimCorey (Year of upload) Intro to Windows Forms (WinForms) in .NET 6. YouTube. Available at: https://www.youtube.com/watch?v=0zLZQesgV5o (Accessed: Day Month Year).
 -  ProgrammingKnowledge2 (2023) Create Your First C# Windows Forms Application using Visual Studio. YouTube. Available at: https://www.youtube.com/watch?v=JSJ1JI2alJg (Accessed: Day Month Year).
    
# Screenshots
-  <img width="1366" height="768" alt="Screenshot 2026-05-29 193543" src="https://github.com/user-attachments/assets/cd9bf4a7-64aa-43dd-b6d2-f6cf5b9386dd" /> Programm greets he user and asks for their name
- <img width="1366" height="768" alt="Screenshot 2026-05-29 193822" src="https://github.com/user-attachments/assets/cee00b77-0b83-485b-a30f-418c543c920b" />
Program provides available topics
-  <img width="1366" height="768" alt="Screenshot 2026-05-29 194024" src="https://github.com/user-attachments/assets/edfc1fc7-44be-4d8c-8933-512f1d8b72d0" />
user asks for questions
-  <img width="1366" height="768" alt="Screenshot 2026-05-29 193846" src="https://github.com/user-attachments/assets/8070693d-9be9-4176-8aaa-5bf6e8d2fb3f" />
error validation for empty insert
-<img width="1366" height="768" alt="Screenshot 2026-05-29 194231" src="https://github.com/user-attachments/assets/cb3a15e3-ac73-4fa8-a791-b065e5442f0d" />

# improvements screenshots
- <img width="883" height="595" alt="Screenshot 2026-06-26 215511" src="https://github.com/user-attachments/assets/2ffc0f4f-6de5-47df-bc62-5a8082b2b121" />
- <img width="786" height="543" alt="Screenshot 2026-06-26 215700" src="https://github.com/user-attachments/assets/84e2150b-611d-47c3-b26b-9f584ef39284" />
- <img width="784" height="543" alt="Screenshot 2026-06-26 215624" src="https://github.com/user-attachments/assets/c6f63477-f4a3-40cc-bd51-9eb0e20fba44" />
- <img width="885" height="593" alt="Screenshot 2026-06-26 215751" src="https://github.com/user-attachments/assets/1b6f0124-5c09-4cea-92c0-f7511d2339b5" />
- <img width="877" height="588" alt="Screenshot 2026-06-26 215728" src="https://github.com/user-attachments/assets/8d52e2e1-16b8-425d-8add-b35cc327159c" /> 



  



 



