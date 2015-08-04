//  eliza.fs
// 
//  Author: Dakota Murray
//  Date:   15 November, 2014
//  For:    CS 3490 - Programming Languages
// 
//  This file simulates and begins execution of the "Eliza Program" as detailed
//  in the class textbook. The eliza program continuously accepts input from a 
//  user and acts as a "georgian therapist" by contantly responsing to the user's
//  input with questions containing words the user used in their input.
// 
//  An example of this would be as follows:
//  user input> i am sad
//  response  > I am sorry that you are sad
//  user input> you hate me
//  response  > Why do you think I hate you?
//  ...
// 
//  In response to Exercise 5.18:
//  ---------------------------------------------------
//  Some strings which contain the word "mother" but would not respond with the 
//  "Tell me more about your family respone" are listed below:
//   - "my mother hates me"
//   - "you mother me"
//   - "i hate my mother"
//
//  These issues are resolved by moving the "mother" condition to the top of the match
//  sequence so that it is always checked first.
//
//  The "Mother" conditon is still not perfect. there are many strings which will not
//  elicit the correct response from this condition.
//

open System
open System.Text.RegularExpressions

module Eliza =

    // function ElizaMatch
    //
    // Description     - Performs the regular expression matching betwenn 
    //                   a supplied pattern and a user's input string. 
    //
    // Params sentence - a string representing the regular expression pattern to match
    //                   the user's input string against.
    //        inputSentence - the user's input string
    //
    // Returns - If REGEX match was a success, returns a list of all elements to be
    //           'kept' from the input string. Otherwise returns none.
    //
    let (|ElizaMatch|_|) (pattern:string) inputSentence = 
        let result = Regex.Match(inputSentence, pattern)
        if result.Success
            then Some (List.tail [for g in result.Groups -> g.Value])
            else None

    // function matchInput
    //
    // Description    - Matches a user supplied string with a series of regular expression
    //                  patterns. A string will be created and returned containing words
    //                  used in the input string. Performs the majority of the work for
    //                  the Eliza georgian therapist program.
    //
    // Param sentence - a string representing user input
    //
    // Returns        - A string response containing words from the user's input string 
    //                  or a hard coded response if the user's input did not match any
    //                  specified patterns.
    // 
    let matchInput (sentence:string) = 
        let response = 
            match sentence with
            | ElizaMatch "^.*\s+mother\s*.*$" elems ->
                "Tell me more about your family"
            | ElizaMatch "^.*my\s+(\w+)\s+.*me.*$" elems ->
                "Tell me about your " + (List.nth elems 0).ToString()
            | ElizaMatch "^.*i\s+am\s+(.*)$" elems ->
                "I am sorry that you are " + (List.nth elems 0).ToString()
            | ElizaMatch "^.*am\s+i\s+(.*)$" elems -> 
                "Do you believe you are " +  (List.nth elems 0).ToString()
            | ElizaMatch "^.*you\s+(.*)\s+me.*$" elems -> 
                "Why do you think I " +  (List.nth elems 0).ToString() + " you"
            | ElizaMatch "^.*i\s+(.*)my\s+(.*)$" elems -> 
                "Why do you " + (List.nth elems 0).ToString() + 
                "your " + (List.nth elems 1).ToString()
            | ElizaMatch "^.*\s*what is (.*)$" elems -> 
                "What do you think " +  (List.nth elems 0).ToString() + " is?"
            | "yes" | "no" -> "Please continue"
            | _ -> if System.Random().Next(0, 2) = 0
                    then "Go On"
                    else "In what way?" 
        printfn "%A\n" response

    // function eliza
    //
    // Description - Entry pointi function for the Eliza program. Continuously accepts
    //               user input and sends it to be pattern matched and responded to. If
    //               the user enters a blank string then the program print "Goodbye" and
    //               terminate. 
    //
    let rec eliza() = 
        let sentence = Console.ReadLine()
        if sentence <> "" then
            matchInput sentence
            eliza()
        else
            printfn "Goodbye"
            System.Environment.Exit;

    //Calls the entry point function
    let test = eliza()
