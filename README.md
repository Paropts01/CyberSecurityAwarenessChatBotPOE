# Cyber Security Awareness Chat Bot

# overview
This project is a WPF desktop application that provides an interactive chatbot to educate users about cybersecurity topics such as password safety, scam detection, privacy protection, and phishing awareness. The bot engages users in a conversational manner, remembers their name and favorite topics, and responds with detailed tips, empathy, and follow-up suggestions.

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

 # Architecture And Key Components
 - MainWindow.xaml.cs = Orchestrates the entire conversation flow: validates input, calls the appropriate manager classes, updates the UI with typing animations, and shows temporary “Thinking…” messages.
 - CyberSecurityChatBot = Stores long, comprehensive answers for the main topics (used when a keyword is detected without an explicit “tip” request).
 - RandomResponseManager =  Holds multiple shorter tips per topic and returns a random one on demand.
 - ConversationManager = Tracks the current topic and detects follow‑up phrases.
 - SentimentDetector = Scans input for emotional keywords and returns an empathetic response.
 - Chatbot = Manages user‑specific data (name and favourite topic).
 - TypingStyle = Provides the character‑by‑character typing effect for messages.
 - AudioPlayer = Plays the greeting sound (handles exceptions gracefully).

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
  
# Usage
- click my github link which will take you to my resipoties then you click the CyberSecurityChatBot-Poe option.
- click the green button that is written code and options will be provided and you shold choose the compressed zip file and download it
- open the microsoft visual studio and open the downloaded zip solution
- Run the application in Visual Studio.
- Enter your name when prompted.
- Ask questions related to cybersecurity topics provided by the bot.
- Type "exit" or "Goodbye" to end the session.

  # Future improvements
  - Add more topics
  - add more advanced features
  - Store user interaction history

 # References
 -  DeepSeek-AI (2025). *DeepSeek-R1: Incentivizing Reasoning Capability in LLMs via Reinforcement Learning*. Available at: https://arxiv.org/abs/2501.12948 (Accessed: [20/05/2026]).
 -  IAmTimCorey (Year of upload) Intro to Windows Forms (WinForms) in .NET 6. YouTube. Available at: https://www.youtube.com/watch?v=0zLZQesgV5o (Accessed: Day Month Year).
 -  ProgrammingKnowledge2 (2023) Create Your First C# Windows Forms Application using Visual Studio. YouTube. Available at: https://www.youtube.com/watch?v=JSJ1JI2alJg (Accessed: Day Month Year).
    
# Screenshots
- <img width="1366" height="768" alt="Screenshot 2026-05-27 192310" src="https://github.com/user-attachments/assets/20151c3b-00fd-47dc-9289-a7791d60f51e" /> Programm greets he user and asks for their name
- <img width="1366" height="768" alt="Screenshot 2026-05-27 192351" src="https://github.com/user-attachments/assets/49461923-aac2-46f4-8840-5b85e2dde978" /> Program provides available topics
- <img width="1366" height="768" alt="Screenshot 2026-05-27 192631" src="https://github.com/user-attachments/assets/62f78206-6e32-4b0e-b2eb-02a6b1bc49d5" /> user asks for questions
- <img width="1366" height="768" alt="Screenshot 2026-05-27 192655" src="https://github.com/user-attachments/assets/5c539183-d3da-4e45-81b6-908ba37c65f7" /> error validation for empty insert
-  <img width="1366" height="768" alt="Screenshot 2026-05-27 192910" src="https://github.com/user-attachments/assets/39da01b9-acb6-4e5e-b6ac-5882ac6dcd32" />
 



