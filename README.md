# ⚡ Cybersecurity Chatbot App
An interactive desktop assistant that identifies security topics, tracks user sentiment, manages task automation, and runs educational evaluation modules.

## Student Information
* **Name:** Koketso Modiselle
* **Student Number:** ST10399194
* **Coursework:** PROG6221 POE (Full Solution Build)

---

## Full List of Features Across Parts 1, 2, and 3

### 💻 Part 1: Console Application Baseline
* **Command Line Processing:** Baseline C# terminal infrastructure processing raw user text token strings.
* **Basic Text Matching:** Evaluated core cybersecurity topic keywords (Phishing, 2FA, Malware, Passwords) through standard input/output streams.

### 🎨 Part 2: WPF Desktop Migration & Core GUI Architecture
* **Terminal Interface Layout:** Migrated the background logic into a dark cyberpunk themed WPF desktop user interface using native XAML layout panels.
* **Interactive Message Feed:** Implemented a central `RichTextBox` conversation feed utilizing asynchronous character-by-character text rendering (`Task.Delay`) to replicate life-like support interactions.
* **Automated Viewport Locking:** Integrated runtime focus listeners to force the chat panel canvas to instantly down-scroll on new message entries.
* **Emotion Analysis (`SentimentDetector.cs`):** Processes text patterns to gauge user emotions and dynamically alters conversation tone responses.
* **Session Tracking (`MemoryStore.cs`):** Captures and persists session parameters like user names and configuration variables for contextual callbacks.

### 💾 Part 3: JSON Persistence & Advanced Operation Panels
* **Persistent Data Storage:** Integrated data modeling arrays (`CyberTask.cs`) mapping straight to a physical disk file network (`tasks.json`).
* **Complete CRUD Lifecycle Control:** Full dashboard controls built into a multi-panel `TabControl` block supporting task addition, selective completion flags, and record deletion.
* **App Startup Auto-Hydration:** Enforces assignment constraints by forcing the system to automatically load and parse all stored JSON records into the UI grid right at application startup.
* **Cybersecurity Quiz Engine (`QuizManager.cs`):** Tracks performance metrics across an interactive 16-question bank spanning 8 discrete defense categories. Automatically hides unneeded choices for True/False options and streams inline corrective text directly into the chat display.
* **NLP Intention Extractor:** Intercepts message phrasings using keyword logic loops to pull parameters out of user chat text to trigger automated backend task additions without manual button clicks.
* **Paginated Activity Auditing Logs (`ActivityLogger.cs`):** Logs application modifications alongside chronological timestamps (`[HH:mm]`), limiting active display outputs to 5 entries to prevent interface clutter while supporting an explicit `show more` view extension command.

---

## Prerequisites
* **Development Environment:** Visual Studio 2022
* **Target Framework Runtime:** .NET 8.0 SDK
* **External Package Library:** `Newtonsoft.Json` NuGet Package (v13.0.3 or higher)

---

## Step-by-Step Setup Instructions

### 1. Install the Newtonsoft.Json NuGet Package
1. Open your project solution within **Visual Studio 2022**.
2. From the top menu bar, select **Tools** ➔ **NuGet Package Manager** ➔ **Manage NuGet Packages for Solution...**
3. Switch to the **Browse** tab and search for `Newtonsoft.Json`.
4. Select the package name, check the box next to your project in the right-hand panel, and click **Install**.

### 2. Database Execution Note (tasks.json auto-creation)
* **No Manual Setup Needed:** You do not need to create or configure a `tasks.json` file manually. The application automatically handles file generation inside your execution folder the exact moment your first task is added via the dashboard or chat input box.

### 3. Where to Place greeting.wav
* **Asset Mapping Rule:** Place your asset file named **`greeting.wav`** directly inside your project's main output execution directory:
  `[Your Project Repository Folder]/bin/Debug/net8.0-windows/`
* *Note: This path configuration prevents execution faults when the media player initialization routine triggers on application launch.*

### 4. How to Run the Project
1. Open Visual Studio 2022 and launch the primary solution file (`CybersecurityChatbot.sln`).
2. Verify that the solution target dropdown box on the top toolbar is set to **Debug**.
3. Press the **F5** hotkey or click the green **Start / Debug Arrow** button to build and run the application.

---

## Project Deliverables & Verification

### Screenshot of the Running GUI
![Running Chatbot Application App](gui_screenshot.png)

### Screenshot of the GitHub Actions Green Tick
![GitHub Actions Workflow Status](github_actions_tick.png)

### YouTube Video Link (Unlisted)
* Unlisted Demonstration Walkthrough Video: https://youtu.be

---

## List of All Three Releases with Descriptions

### 📌 Release Milestone v3.0 (Part 1 & Part 2 Baseline)
* **Description:** Represents the complete migration from the original Console App baseline into the active WPF interface layout. Features responsive custom chat bubbles, asynchronous typing animations, user sentiment profiling, and workspace session memory tracking.

### 📌 Release Milestone v3.1 (Part 3 Task Persistence)
* **Description:** Milestone tracking the core integration of structural task managers and local storage pipelines. Deploys background JSON file serialization mechanisms (`tasks.json`) with an automatic loading loop execution sequence running instantly at system startup.

### 📌 Release Milestone v3.2 (Final Integration Build)
* **Description:** The final complete project assembly version. Integrates the 16-question cybersecurity quiz matrix, automated string-based NLP intention extraction routers, chronological auditing activity logs with paginated history limits, and an updated multi-tab layout structure.
