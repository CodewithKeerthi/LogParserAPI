# LogParserAPI

A simple API to parse `.log` or `.txt` files and extract lines as:

- Errors
- Warnings
- Info

## Project Structure

- `Services/` — Core logic to read and parse the log file.
- `Controllers/` — Optional API controller to expose the parser.
- `Program.cs` / `Startup.cs` — Default .NET API startup.

## Usage

You can test by placing a log file in the configured path and hitting the `/api/logparser/parse` endpoint (if using the controller).