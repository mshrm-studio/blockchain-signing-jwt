const getProvider = () => {
    if ('phantom' in window) {
        const provider = window.phantom?.solana;
        if (provider?.isPhantom) {
            return provider;
        }
    }

    window.open('https://phantom.app/', '_blank');
};

const toHexString = (byteArray) => {
    return Array.from(byteArray, function (byte) {
        return ('0' + (byte & 0xFF).toString(16)).slice(-2);
    }).join('')
}

const signMessage = async () => {
    const provider = getProvider();
    if (!provider) {
        console.log('Phantom not detected');
        return;
    }

    try {
        const resp = await provider.connect();

        const raw_message = new Date().toISOString();
        const encodedMessage = new TextEncoder().encode(raw_message);
        const signedMessage = await provider.signMessage(encodedMessage, "utf8");
        const network = "Solana";
        const address = resp.publicKey.toString();
        const signature = toHexString(signedMessage.signature);

        console.log("Public key: " + address);
        console.log("Signature (hex): " + signature);
        console.log("Message: " + raw_message);

        // Send to back-end
        const response = await fetch('http://localhost:5191/blockchain/token', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ raw_message, signature, network, address }),
        });
    
        const result = await response.json();
        console.log(result);

    } catch (error) {
        console.error(error);
    }
};

await signMessage();