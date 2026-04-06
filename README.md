<a id="readme-top"></a>

[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/MontoyaTM/MovieTime">
    <img src="wwwroot/images/MontoyaTM.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">Movie Time</h3>

  <p align="center">
    MovieTime is  a client-side movie discovery application using C# and .NET 8 with Blazor WebAssembly, integrating The Movie Database (TMDB) API to enable users to search, browse popular and in-theater films, and view detailed movie information (cast, ratings, trailers, and more). 
    Implemented local storage to persist user-selected favorite movies and designed a responsive, mobile-friendly UI using Bootstrap. Ensured secure API access, intuitive navigation, and robust handling of loading, error, and empty states.
    <br />
    <br />
    <br />
  </p>

[Visit Website](https://blazormovietime.netlify.app)
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## About The Project

<div align="center">
    <img src="wwwroot/images/screenshot/MovieTime_.png" alt="Logo" width="2560" height="1440">
</div>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

- [![Blazor](https://img.shields.io/badge/Blazor-512BD4?logo=blazor&logoColor=fff)](#)
- [![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](#)
- [![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?logo=bootstrap&logoColor=fff)](#)
- [![HTML](https://img.shields.io/badge/HTML-%23E34F26.svg?logo=html5&logoColor=white)](#)
- [![CSS](https://img.shields.io/badge/CSS-639?logo=css&logoColor=fff)](#)


<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->

## Getting Started

To get a local copy up and running follow these simple example steps.

### Installation

1. Get a free API Key at [https://www.themoviedb.org/](https://www.themoviedb.org/)
2. Clone the repo
   ```sh
   git clone https://github.com/MontoyaTM/MovieTime.git
   ```
3. Install NPM packages
   ```sh
   npm install
   ```
4. Enter your API in `appsettings.Development.json` or in your `manage user secrets`
   ```
   "TMDB_Key": = "ENTER YOUR API";
   ```
5. Change git remote url to avoid accidental pushes to base project
   ```sh
   git remote set-url origin github_username/repo_name
   git remote -v # confirm the changes
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->

## Contact

Ricardo Montoya - montoyar.sw.dev@gmail.com

Project Link: [https://github.com/MontoyaTM/MovieTime](https://github.com/MontoyaTM/MovieTime)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/ricardomontoya01 
[product-screenshot]: images/screenshot.png
