# 🧩 Technical Documentation  
## Project: TCP Key Verification Server

### 1. Overview
This server implements a simple **key exchange and verification** protocol between the server and a client.  
After the client connects, the server generates and sends a random 16-character hexadecimal key.  
The client must send back the exact same key for verification.  
If the keys match — the client is successfully verified.

---

### 2. Connection Parameters
| Parameter | Value |
|------------|--------|
| **Protocol** | TCP |
| **IP Address** | `192.168.1.77` |
| **Port** | `8686` |
| **Encoding** | UTF-8 |
| **Key Length** | 16 characters (HEX) |

---

### 3. Workflow
#### 🔹 On the Server:
1. Start a TCP listener on the specified IP and port.  
2. Wait for a client to connect.  
3. Generate a random HEX key (e.g. `A4B8D923E19F56C0`).  
4. Send the generated key to the connected client.  
5. Wait for the client to send its key back.  
6. Compare the two keys:
   - If they **match**, send byte `1` (verification success).  
   - If they **don’t match**, send byte `0` (verification failed).  
7. Close all network connections.

#### 🔹 On the Client:
1. Connect to the server at `192.168.1.77:8686`.  
2. Receive the 16-character HEX key from the server.  
3. Send the same key back to the server.  
4. Receive a single-byte response:
   - `1` → Verification successful.  
   - `0` → Verification failed.

---

4. Technical Details

Language: C# (.NET 8)

Classes Used: TcpListener, TcpClient, NetworkStream

Asynchronous Model: async/await for non-blocking I/O

Encoding: UTF-8 for text serialization

Key Generator: Random 16-character string using HEXDIGITS = "0123456789ABCDEF"