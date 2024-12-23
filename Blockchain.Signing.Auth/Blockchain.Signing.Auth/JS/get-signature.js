async function signMessage() {
  if (typeof window.ethereum !== 'undefined') {
    try {
      // Request account access
      await window.ethereum.request({ method: 'eth_requestAccounts' });
      
      const accounts = await window.ethereum.request({ method: 'eth_accounts' });
      const account = accounts[0];

      const message = new Date();
      
      // Sign the message
      const signature = await window.ethereum.request({
        method: 'personal_sign',
        params: [message, account, 'Example password'],
      });

      console.log("Signature: " + signature);
        console.log("Account: " + account);
        console.log("Message: " + message);

      // Send to back-end
      /*const response = await fetch('/api/verify', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ message, signature, address: account }),
      });*/


      const result = await response.json();
      console.log(result);
    } catch (error) {
      console.error(error);
    }
  } else {
    console.log('MetaMask not detected');
  }
}