using System;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections;
using Searcharoo.Common;

namespace Searcharoo.Engine
{
    public class Search
    {
        #region Private Fields: _Stemmer, _Stopper, _GoChecker, _DisplayTime, _Matches
        /// <summary>Stemmer to use</summary>
        private IStemming _Stemmer;
        /// <summary>Stopper to use</summary>
        private IStopper _Stopper;
        /// <summary>Go word parser to use</summary>
        private IGoWord _GoChecker;
        /// <summary>Display string: time the search too</summary>
        private string _DisplayTime;
        /// <summary>Display string: matches (links and number of)</summary>
        private string _Matches = "";
        #endregion

        #region Public Properties: SearchQueryMatchHtml, DisplayTime
        public string SearchQueryMatchHtml
        {
            get { return _Matches; }
            set { _Matches = value; }
        }
        
        public string DisplayTime
        {
            get { return _DisplayTime; }
            set { _DisplayTime = value; }
        }
        #endregion


        public SortedList GetResults(string searchterm, Catalog catalog)
        {
            SortedList output = new SortedList();

            // ----------------------- DOING A SEARCH ----------------------- 
            if ((null != searchterm) && (null != catalog))
            {
                SetPreferences();

                string[] searchTermArray = null, searchTermDisplay = null;

                /****** Too *********/
                Regex r = new Regex(@"\s+");            //remove all whitespace
                searchterm = r.Replace(searchterm, " ");// to a single space
                searchTermArray = searchterm.Split(' '); // then split
                searchTermDisplay = (string[])searchTermArray.Clone();
                for (int i = 0; i < searchTermArray.Length; i++)
                {
                    if (_GoChecker.IsGoWord(searchTermArray[i]))
                    {	// was a Go word, just Lower it
                        searchTermArray[i] = searchTermArray[i].ToLower();
                    }
                    else
                    {	// Not a Go word, apply stemming
                        searchTermArray[i] = searchTermArray[i].Trim(' ', '?', '\"', ',', '\'', ';', ':', '.', '(', ')').ToLower();
                        searchTermArray[i] = _Stemmer.StemWord(searchTermArray[i].ToString());
                    }
                }

                if (searchterm == String.Empty)
                {
                    // After trimming the search term, it was found to be empty!
                    return output;
                }
                else
                {	// we have a search term!
                    DateTime start = DateTime.Now;  // to show 'time taken' to perform search

                    // Array of arrays of results that match ONE of the search criteria
                    Hashtable[] searchResultsArrayArray = new Hashtable[searchTermArray.Length];
                    // finalResultsArray is populated with pages that *match* ALL the search criteria
                    HybridDictionary finalResultsArray = new HybridDictionary();

                    bool botherToFindMatches = true;
                    int indexOfShortestResultSet = -1, lengthOfShortestResultSet = -1;

                    for (int i = 0; i < searchTermArray.Length; i++)
                    {	// ##### THE SEARCH #####
                        searchResultsArrayArray[i] = catalog.Search(searchTermArray[i].ToString());
                        if (null == searchResultsArrayArray[i])
                        {
                            _Matches += searchTermDisplay[i] + " <font color=gray style='font-size:xx-small'>(not found)</font> ";
                            botherToFindMatches = false; // if *any one* of the terms isn't found, there won't be a 'set' of Matches
                        }
                        else
                        {
                            int resultsInThisSet = searchResultsArrayArray[i].Count;
                            _Matches += "<a href=\"?" + Preferences.QuerystringParameterName + "=" + searchTermDisplay[i] + "\" title=\"" + searchTermArray[i] + "\">"
                                    + searchTermDisplay[i]
                                    + "</a> <font color=gray style='font-size:xx-small'>(" + resultsInThisSet + ")</font> ";
                            if ((lengthOfShortestResultSet == -1) || (lengthOfShortestResultSet > resultsInThisSet))
                            {
                                indexOfShortestResultSet = i;
                                lengthOfShortestResultSet = resultsInThisSet;
                            }
                        }
                    }

                    // Find the common files from the array of arrays of documents
                    // matching ONE of the criteria
                    if (botherToFindMatches)                                            // all words have *some* matches
                    {																	// for each result set [NOT required, but maybe later if we do AND/OR searches)
                        int c = indexOfShortestResultSet;                               // loop through the *shortest* resultset
                        Hashtable searchResultsArray = searchResultsArrayArray[c];

                        foreach (object foundInFile in searchResultsArray)             // for each file in the *shortest* result set
                        {
                            DictionaryEntry fo = (DictionaryEntry)foundInFile;          // find matching files in the other resultsets

                            int matchcount = 0, totalcount = 0, weight = 0;

                            for (int cx = 0; cx < searchResultsArrayArray.Length; cx++)
                            {
                                totalcount += (cx + 1);                                // keep track, so we can compare at the end (if term is in ALL resultsets)
                                if (cx == c)                                      // current resultset
                                {
                                    matchcount += (cx + 1);                          // implicitly matches in the current resultset
                                    weight += (int)fo.Value;                       // sum the weighting
                                }
                                else
                                {
                                    Hashtable searchResultsArrayx = searchResultsArrayArray[cx];
                                    if (null != searchResultsArrayx)
                                    {
                                        foreach (object foundInFilex in searchResultsArrayx)
                                        {   // for each file in the result set
                                            DictionaryEntry fox = (DictionaryEntry)foundInFilex;
                                            if (fo.Key == fox.Key)
                                            {
                                                matchcount += (cx + 1);               // and if it matches, track the matchcount
                                                weight += (int)fox.Value;           // and weighting; then break out of loop, since
                                                break;                              // no need to keep looking through this resultset
                                            }
                                        } // foreach
                                    } // if
                                } // else
                            } // for
                            if ((matchcount > 0) && (matchcount == totalcount))		// was matched in each Array
                            {   // we build the finalResults here, to pass to the formatting code below
                                // - we could do the formatting here, but it would mix up the 'result generation'
                                // and display code too much
                                fo.Value = weight; // set the 'weight' in the combined results to the sum of individual document matches
                                if (!finalResultsArray.Contains(fo.Key)) finalResultsArray.Add(fo.Key, fo);
                            } // if
                        } // foreach
                    }


                    // Time taken calculation
                    Int64 ticks = DateTime.Now.Ticks - start.Ticks;
                    TimeSpan taken = new TimeSpan(ticks);
                    if (taken.Seconds > 0)
                    {
                        _DisplayTime = taken.Seconds + " seconds";
                    }
                    else if (taken.TotalMilliseconds > 0)
                    {
                        _DisplayTime = Convert.ToInt32(taken.TotalMilliseconds) + " milliseconds";
                    }
                    else
                    {
                        _DisplayTime = "less than 1 millisecond";
                    }

                    // The preceding 80 lines (or so) replaces this single line from Version 1
                    //       Hashtable searchResultsArray = m_catalog.Search (searchterm);
                    // when only single-word-searches were supported. Look closely and you'll see this line
                    // labelled #THE SEARCH# still in the code above...

                    // Format the results
                    if (finalResultsArray.Count > 0)
                    {	// intermediate data-structure for 'ranked' result HTML
                        //SortedList 
                        output = new SortedList(finalResultsArray.Count); // empty sorted list
                        //                DictionaryEntry fo;
                        ResultFile infile;
                        //                string result="";
                        int sortrank = 0;

                        // build each result row
                        foreach (object foundInFile in finalResultsArray.Keys)
                        {
                            // Create a ResultFile with it's own Rank
                            infile = new ResultFile((File)foundInFile);

                            // Jim Harkins [sort for paging] ported from VB to C#
                            // http://www.codeproject.com/aspnet/spideroo.asp#xx927327xx
                            infile.Rank = (int)((DictionaryEntry)finalResultsArray[foundInFile]).Value;
                            sortrank = infile.Rank * -1000;		// Assume not 'thousands' of results
                            if (output.Contains(sortrank))
                            { // rank exists - drop key index one number until it fits
                                for (int i = 1; i < 999; i++)
                                {
                                    sortrank++;
                                    if (!output.Contains(sortrank))
                                    {
                                        output.Add(sortrank, infile);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                output.Add(sortrank, infile);
                            }
                            sortrank = 0;	// reset for next pass
                        }
                        // Jim Harkins [paged results]
                        // http://aspnet.4guysfromrolla.com/articles/081804-1.aspx
                    } // else Count == 0, so output SortedList will be empty
                } 
            }
            return output;
        }

        private void SetPreferences()
        {
            // Set-up Stemming (if required)
            switch (Preferences.StemmingMode)
            {
                case 1:
                    _Stemmer = new PorterStemmer();	//Stemmer = new SnowStemming();
                    break;
                case 2:
                    _Stemmer = new PorterStemmer();
                    break;
                default:
                    _Stemmer = new NoStemming();
                    break;
            }

            switch (Preferences.GoWordMode)
            {
                case 1:
                    _GoChecker = new ListGoWord();
                    break;
                default:
                    _GoChecker = new NoGoWord();
                    break;
            }
        }
    }
}
