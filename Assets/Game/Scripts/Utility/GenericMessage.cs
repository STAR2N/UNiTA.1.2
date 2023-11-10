/*
MIT License

Copyright (c) 2021 Chan-yeong Kang

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

/*
 * Generic Message System
 * A message system with generic argument holder as route handler.
 * 
 * Purpose:
 *  Message queueing.
 *  Route receiver by argument holder.
 *  Trace reference where receiver and caller using some feature like intellisense.
*/

namespace MessageSystem {
    public class GenericMessage<T> where T : System.EventArgs {
        public delegate void MessageEventHandler(T args);
        public static event MessageEventHandler OnMessage = delegate { };

        public static void Invoke(T args) {
            OnMessage.Invoke(args);
        }
    }
}