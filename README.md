# Market Tracker 🚀

Market Tracker is service designed to help users track the variation of prices across different stores.

## Features 🌟

- **Variation Analysis**: Users can view the variation of prices over time for a specific item.
- **Store Comparison**: Users can compare prices of an item across different stores.
- **Account system**: The app supports an account system to allow users to save their preferences and track their favorite items.

## Technologies Used 💻

- **Backend**: C# with ASP.NET Core
- **Frontend**: Next.js with React
- **Database**: PostgreSQL
- **Authentication**: JSON Web Tokens (JWT)
- **API Documentation**: Swagger UI

## About Us 🙋‍♂️

This app was developed by a team of students at ISEL (Instituto Superior de Engenharia de Lisboa) as part of the Software Engineering course. The team members are:

- André Graça (47224)
- Diogo Santos (48459)
- Daniel Caseiro (46052)

## Getting Started 🛠️

### Prerequisites

- Node.js and npm installed on your machine.
- PostgreSQL installed and running on your machine.

### Installation 🔧

1. Clone the repository:

   ```bash
   git clone https://github.com/AndreGraca3/Market-Tracker.git
   ```

2. Navigate to the project directory:

   ```bash
   cd Market-Tracker
   ```

3. Install the required dependencies:

   ```bash
   cd src/MarketTracker.WebApi
   dotnet restore
   cd ../MarketTracker.WebApp
   npm install
   ```

### Configuration ⚙️

1. Backend Configuration:
   TBD

2. Frontend Configuration:

   - Rename the `.env.example` to `.env` in the `Market-Tracker.WebApp` directory with the following content:

     ```env
     TBD
     ```

### Running the App 🏃‍♂️

1. Start the backend server:

   ```bash
   cd src/MarketTracker.WebApi
   dotnet run
   ```

2. Start the frontend server:

   ```bash
   cd src/MarketTracker.WebApp
   npm run dev
   ```
