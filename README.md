# Pigeon

Pigeon is a monolotic clean architecture .Net dual-purpose project designed to help website owners monitor and manage the status of their web pages and aslo perform data mining to extract insights from news publisher websites.

## Features

### Part 1: Web Crawler and Monitoring
- Crawls websites to extract mentioned links on a page. [Link Crawler.cs](https://github.com/abowfzl/Pigeon/blob/master/Pigeon/Crawler/LinkCrawler.cs)
- Background job fetches and logs page response time and status code.
- Ticketing system for admins to assign crashed pages to team members and track resolutions (Admin interface).
- Login and user management with [Asp .Net Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0)
- 
### Part 2: Data Mining
- Extracts tags from news websites.
- the `Apriori` [algorithm](https://github.com/abowfzl/Pigeon/tree/master/Pigeon/Algoritms) implementation to find relationships between tags. check this page [DataMining.cs](https://github.com/abowfzl/Pigeon/blob/master/Pigeon/Pages/DataMining.cshtml.cs)

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/abowfzl/Pigeon.git
    ```
2. Navigate to the project directory:
    ```sh
    cd Pigeon
    ```
3. Install dependencies:
    ```sh
    dotnet restore
    ```

## Usage

1. Build and run the project:
    ```sh
    dotnet build
    dotnet run
    ```
2. The application will automatically start the web crawler, logging, and data mining processes.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch:
    ```sh
    git checkout -b feature/YourFeature
    ```
3. Make your changes.
4. Commit your changes:
    ```sh
    git commit -m 'Add some feature'
    ```
5. Push to the branch:
    ```sh
    git push origin feature/YourFeature
    ```
6. Open a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any questions or inquiries, please contact Abolfazl Moslemian at [moslemianabolfazl@gmail.com].
