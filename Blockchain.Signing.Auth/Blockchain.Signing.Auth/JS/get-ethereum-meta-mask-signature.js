const signMessage = async () => {
    if (typeof window.ethereum !== 'undefined') {
        try {
            await window.ethereum.request({ method: 'eth_requestAccounts' });

            const accounts = await window.ethereum.request({ method: 'eth_accounts' });
            const account = accounts[0];
            const raw_message = new Date().toISOString();
            const network = "Ethereum";

            const signature = await window.ethereum.request({
                method: 'personal_sign',
                params: [message, account],
            });

            console.log("Signature: " + signature);
            console.log("Account: " + account);
            console.log("Message: " + raw_message);

            // Send to back-end
            /*
            const response = await fetch('/api/verify', {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
              },
              body: JSON.stringify({ raw_message, signature, network }),
            });

            const result = await response.json();
            console.log(result);
            */

        } catch (error) {
            console.error(error);
        }
    } else {
        console.log('MetaMask not detected');
    }
}

await signMessage();