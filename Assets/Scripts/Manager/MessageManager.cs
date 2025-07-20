using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : BaseManager<MessageManager>
{
    private Dictionary<MessageType, List<EventHandler<EventArgs>>> _messageListeners = new Dictionary<MessageType, List<EventHandler<EventArgs>>>();

    public enum MessageType
    {
        PlayerEnterPlatformTrigger,
        PlayerLeavePlatformTrigger,
    }

    public void RegistHandler(MessageType messageType, EventHandler<EventArgs> handler)
    {
        if (handler == null)
            return;
        if (_messageListeners.ContainsKey(messageType))
        {
            _messageListeners[messageType].Add(handler);
        }
        else
        {
            _messageListeners[messageType] = new List<EventHandler<EventArgs>>() { handler };
        }
    }

    public void UnRegistHandler(MessageType messageType, EventHandler<EventArgs> handler)
    {
        if (_messageListeners.ContainsKey(messageType))
        {
            _messageListeners[messageType].Remove(handler);
        }
    }

    public void BoardcastMessage(MessageType messageType, object sender = null, EventArgs args = null)
    {
        if (_messageListeners.ContainsKey(messageType))
        {
            foreach (var handler in _messageListeners[messageType])
            {
                try
                {
                    handler(sender, args);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error when boardcast message {messageType} : {e.Message}");
                    continue;
                }
            }

        }
    }

    public void ClearMessageType(MessageType msg)
    {
        if (_messageListeners.ContainsKey(msg))
        {
            _messageListeners[msg].Clear();
        }
    }

    public void ClearAllHandlers()
    {
        _messageListeners.Clear();
    }
}
