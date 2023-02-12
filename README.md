# ChatGPTMosaic

This is one of my ChatGPT conversations I had recently. This demonstrates how to generate a coding solution and then, within the same context, ask ChatGPT to fix and improve upon it's previous attempted solution(s) to arrive at a (mostly) working solution.

[See the full ChatGPT conversation here](ChatGPTConversation.md)

Overall I am VERY impressed with the quality of code generation, language processing, and contextual conversation abilities of ChatGPT. For example, after prompting ChatGPT to generate a .Net 6.0 version of the code, the resulting code had a reference to "Image.FromFile", which is from System.Drawing namespace in previous versions of .Net but not available in .Net 6.0. My next comment was, "Image.FromFile is not part of .NET 6.0, so that won't work." ChatGPT then apologized, pointed out that I was correct, and then revised the code to be fully .Net 6.0 compliant. 😲
# Projects in this repository

- ChatGPTMosaicWindows.csproj
  - This is the first program generated, which targeted .Net 4.x. There were a few fixes that had to be done, which you can see in the git commit history.

- ChatGPTMosaic.csproj
  - Once I got the .Net 4.x application working, I asked ChatGPT if it could target .Net 6.0 instead. It obliged and the result is the ChatGPTMosaic project. Again, some fixes had to be manually applied, which can be seen int he git commit history.

Date of conversation: February 11 2023.

The above ChatGPT chat log markdown file was generated (and then some minor corrections applied) with the [ChatGPT Desktop Application](https://github.com/lencx/ChatGPT) open source project found on GitHub.
