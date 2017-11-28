#if MATCH_UP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MatchUp;

/**
 * Utilize the MatchUp Matchmaker to perform matchmaking. 
 * Connect to the match host using NAT Traversal
 */
[RequireComponent(typeof(NetworkManager))]
public class ExampleMatchUp : Matchmaker
{
    NATTraversal.NetworkManager netManager;
    Match[] matches;
    bool showDisconnectButton;

	void Awake()
    {
        netManager = GetComponent<NATTraversal.NetworkManager>();
	}

    void OnGUI()
    {
        if (NetworkManagerExtension.externalIP == null) GUI.enabled = false;
        else GUI.enabled = true;

        if (!NetworkServer.active && !NetworkClient.active)
        {
            // Host a match
            if (GUI.Button(new Rect(10, 10, 150, 48), "Host"))
            {
                StartCoroutine(HostAMatch());
            }

            // List matches
            if (GUI.Button(new Rect(10, 60, 150, 48), "Search matches"))
            {
                Debug.Log("Fetching match list");

                // Filter so that we only receive matches with an eloScore between 150 and 350, and 
                // the Region is North America and the Game type is Capture the Flag
                var filters = new List<MatchFilter>() {
                    new MatchFilter("eloScore", 150, MatchFilter.OperationType.GREATER),
                    new MatchFilter("eloScore", 350, MatchFilter.OperationType.LESS),
                };

                // Get the filtered match list. The results will be received in OnMatchList()
                this.GetMatchList(OnMatchList, 0, 10, filters);
            }
        }

        // Disconnect
        if (showDisconnectButton)
        {
            if (GUI.Button(new Rect(10, 110, 150, 48), "Disconnect"))
            {
                showDisconnectButton = false;
                // Stop hosting and destroy the match
                if (NetworkServer.active)
                {
                    Debug.Log("Destroyed match");
                    netManager.StopHost();
                    this.DestroyMatch();
                }

                // Disconnect from the host and leave the match
                else
                {
                    Debug.Log("Left match");
                    netManager.StopClient();
                    this.LeaveMatch();
                }
            }
        }

        // Set some match data
        if (NetworkServer.active)
        {
            showDisconnectButton = true;
            if (GUI.Button(new Rect(10, 160, 150, 48), "Set match data"))
            {
                ExampleSetMatchData();
            }
        }
        if (matches != null && !showDisconnectButton)
        {
            for (int i = 0; i < matches.Length; i++)
            {
                string newString = "";
                foreach (KeyValuePair<string, MatchData> kvp in matches[i].matchData)
                {
                    if (kvp.Key != "port" && kvp.Key != "internalIP" && kvp.Key != "applicationID" &&
                        kvp.Key != "externalIP")
                    {
                        newString += " | " + kvp.Value;
                    }
                }
                if (GUI.Button(new Rect(170, 10 + i * 26, 600, 25), newString))
                {
                    this.JoinMatch(matches[i], OnJoinMatch);
                }
            }
        }
    }

    IEnumerator HostAMatch()
    {
        string matchName = "Layla's Match";

        // Start the host first so that we connect to the Facilitator and get a GUID
        netManager.StartHostAll(matchName, (uint)netManager.maxConnections + 1);

        // Wait for the Facilitator connection
        while (netManager.natHelper.isConnectingToFacilitator) yield return 0;

        // Make sure we actually connected
        if (!netManager.natHelper.isConnectedToFacilitator) yield break;

        // Add the guid to the match data
        var matchData = new Dictionary<string, MatchData>() {
            { "Match Name", matchName },
            { "guid", netManager.natHelper.guid },
            { "eloScore", 200 }
        };

        // Create the Match with the associated MatchData
        this.CreateMatch(netManager.maxConnections + 1, matchData);

        Debug.Log("Created match: " + this.currentMatch.name);
    }

    /**
     * This is called when the match list is received
     */
    void OnMatchList(bool success, Match[] matchesTemp)
    {
        if (!success) return;

        Debug.Log("Received match list.");
        matches = matchesTemp;
    }

    /**
     * This is called when a response is received from a JoinMatch() request
     * Once the match is succesfully joined you have access to all the associated MatchData
     */
    void OnJoinMatch(bool success, Match match)
    {
        if (!success) return;

        Debug.Log("Joined match: " + match.name);
        showDisconnectButton = true;
        
        string externalIP = match.matchData["externalIP"];
        string internalIP = match.matchData["internalIP"];
        int port = match.matchData["port"];
        ulong guid = (ulong)match.matchData["guid"];

        // Connect to the host
        netManager.StartClientAll(externalIP, internalIP, port, guid);
    }
    
    /**
     * An example of setting some match data.
     * 
     * There are a couple of ways to set match data depending on how much you are
     * setting and what your syntax preferences are.
     * 
     * NOTE: If you are setting more than one value at once you should not use Option 1 
     * since it will send a message to the server each time you call it.
     * 
     * NOTE: If you are setting a value very often (like every frame) you probably want to use
     * Option 3 so that you don't send the data until you explicity call UpdateMatchData()
     */
    public void ExampleSetMatchData()
    {
        Debug.Log("Setting match data");

        /**
         * Option 1: Add or set a single match data value and immediately send it to the matchmaking server
         */
        this.SetMatchData("eloScore", 50);

        /**
         * Option 2: Completely replace existing match data and immediately send it to the matchmaking server
         */
        //var newMatchData = new Dictionary<string, MatchData>() {
        //    { "Key1", "value1" },
        //    { "Key2", 3.14159 }
        //};
        //this.SetMatchData(newMatchData);

        /**
         * Option 3a: Add or set several match data values and then send them all at once
         */
        //this.currentMatch.matchData["Key1"] = 3.14159;
        //this.currentMatch.matchData["Key2"] = "works for strings too";
        //this.UpdateMatchData(); // Send the data to the matchmaking server

        /**
         * Option 3b: Alternative syntax to add or set several match data values and then send them all at once.
         */
        //var additionalMatchData = new Dictionary<string, MatchData>() {
        //    { "Key1", "value1" },
        //    { "Key2", 3.14159 }
        //};
        // This will merge the match data you pass in with any existing match data
        //this.UpdateMatchData(additionalMatchData); // Send the data to the matchmaking server
    }
}
#else
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExampleMatchUp : MonoBehaviour
{
    void Start() 
    { 
        Debug.LogError("This example requires the Match Up plugin. Get it here: http://u3d.as/10eJ\nIf you already have Match Up installed you may just need to re-import this script."); 
    }
}
#endif