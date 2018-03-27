module MyLibrary

open System.Security

#light

//
// explode a string into a list of characters.
// Example: "cat" -> ['c'; 'a'; 't']
//
let explode(s:string) =
  [ for c in s -> c ]


//
// implode a list L of characters back into a string.
// Example: implode ['c'; 'a'; 't'] -> "cat"
//
let implode L =
  let sb = System.Text.StringBuilder()
  let ignore = List.map (fun c -> sb.Append (c:char)) L
  sb.ToString()

//
// remove element first occurence of a list and return new list minus that element 
// Example: removeListElement ['a';'b';'c';'c';'d'] -> ['a';'b';'c';'d'] 
//
let rec removeListElement L acc elementToRemove removed=
    match L with
    | [] -> List.rev acc
    | hd::tl -> if ( hd = elementToRemove && not(removed) ) then 
                    removeListElement tl acc elementToRemove true
                else
                    removeListElement tl (hd::acc) elementToRemove removed

//
// returns score for a certain letter for scrabble scoring 
// Example: scoreLetter 'z' -> 10
//
let scoreLetter letter =
    match letter with
    | 'a' -> 1
    | 'b' -> 3
    | 'c' -> 3
    | 'd' -> 2
    | 'e' -> 1
    | 'f' -> 4
    | 'g' -> 2
    | 'h' -> 4
    | 'i' -> 1
    | 'j' -> 8
    | 'k' -> 5
    | 'l' -> 1
    | 'm' -> 3
    | 'n' -> 1
    | 'o' -> 1
    | 'p' -> 3
    | 'q' -> 10
    | 'r' -> 1
    | 's' -> 1
    | 't' -> 1
    | 'u' -> 1
    | 'v' -> 4
    | 'w' -> 4
    | 'x' -> 8
    | 'y' -> 4
    | 'z' -> 10

//
// Initialize:
//
// This function is called ONCE at program startup to initialize any
// data structures in the library.  We use this function to input the
// Scrabble dictionary and build a list of legal Scrabble words.
//
let mutable WordList = []

let Initialize folderPath =
  let alphabetical = System.IO.Path.Combine(folderPath, "alphabetical.txt")
  WordList <- [ for line in System.IO.File.ReadAllLines(alphabetical) -> line ]
  printfn "%A" (List.length WordList)


//
// possibleWords:
//
// Finds all Scrabble words in the Scrabble dictionary that can be 
// spelled with the given letters.  The words are returned as a list
// in alphabetical order.
//
// Example:  letters = "tca" returns the list
//   ["act"; "at"; "cat"; "ta"]
//
let rec PF word letters =
    match word, letters with
    | [], [] -> true
    | [], _ -> true
    | _, [] -> false
    | hd::tl, hd2::tl2 -> if (List.contains(hd) letters) then
                            let newLetterList = removeListElement letters [] hd false
                            PF tl newLetterList
                          else
                            false

let possibleWords letters = 
    let newWordList = List.filter(fun word -> List.length(explode word) <= List.length(explode letters)) WordList
    List.sort (List.filter (fun word -> PF (explode word) (explode letters)) newWordList)


//
// wordsWithScores:
//
// Finds all Scrabble words in the Scrabble dictionary that can be 
// spelled with the given letters.  The words are then "scored"
// based on the value of each letter, and the results returned as
// a list of tuples in the form (word, score).  The list is ordered
// in descending order by score; if 2 words have the same score,
// they are ordered in alphabetical order.
//
// Example:  letters = "tca" returns the list
//   [("act",5); ("cat",5); ("at",2); ("ta",2)]
//
let rec getWordsScore word count=
    match word with
    | [] -> count
    | hd::tl -> getWordsScore tl (count + scoreLetter hd)

let wordsWithScores letters =
    let listofWords = possibleWords letters
    let listofTouples = List.map(fun x -> (x, (getWordsScore (explode x) 0))) listofWords
    List.sortBy(fun (x,y) -> -y) listofTouples
    

//
// wordsThatFitPattern:
//
// Finds all Scrabble words in the Scrabble dictionary that can be 
// spelled with the given letters + the letters in the pattern, such
// that those words all fit into the pattern.  The results are 
// returned as a list of tuples (word, score), in descending order by
// score (with secondary sort on the word in alphabetical order).
//
// Example:  letters = "tca" and pattern = "e**h" returns the list
//   [("each",9); ("etch",9); ("eath",7)]
//
let rec wordFitsPattern patternList wordList letterList=
    match patternList, wordList with
    | [], [] -> true
    | hd::tl, hd2::tl2 -> if( (hd = '*' && List.contains(hd2) letterList )) then
                                let newLetterList = removeListElement letterList [] hd2 false
                                wordFitsPattern tl tl2 newLetterList
                          elif hd = hd2 then
                                wordFitsPattern tl tl2 letterList
                          else
                                false

let PF2 word pattern letters =
    let patternList = explode pattern
    let wordList = explode word
    let letterList = explode letters

    if (List.length patternList = List.length wordList) then
         wordFitsPattern patternList wordList letterList
    else
        false


let wordsThatFitPattern letters pattern = 
    let patternWords = (List.filter (fun word -> PF2 word pattern letters) WordList)
    let sortedPatternWords = List.sort patternWords
    let patternWordsWithPoints = List.map (fun x -> (x, (getWordsScore (explode x) 0))) sortedPatternWords
    List.sortBy(fun (x,y) -> -y) patternWordsWithPoints
