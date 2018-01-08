# Scrabble Help -- Visual Studios Project
Note: C# UI was not written or designed by me

-	Builds List of All Words in Dictionary in Txt Files in Dictionary folder
-	Takes in letters in User Interface and builds all possible words in the dictionary for them through a predicate function
- Predicate function takes in letters and the word and sees if word can be created with the letters given
-	Words are scored through a function that uses tail recursion to add up the score of each letter
-	Words are returned as touples with their scores sorted in descending order
-	Checking if the word actually fits the pattern uses two different functions
- First to eliminate a lot of the words they were checked to see if they had the same length as the pattern
- If not they were immediately discarded, if they were they were run through a different predicate function
- Through this function words were matched up letter by letter removing a letter from each until they were both empty

Ex:
    ![alt text](https://raw.githubusercontent.com/ozorob2/ScrabbleHelp/blob/master/ExampleScreenshot.png)
  
