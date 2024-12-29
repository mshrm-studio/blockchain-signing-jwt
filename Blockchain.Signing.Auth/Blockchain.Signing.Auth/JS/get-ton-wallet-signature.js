const getProvider = () => {
    if (typeof window !== 'undefined' && window.ton) {
        return window.ton;
    }
    return null;
};

const toHexString = (byteArray) => {
    return Array.from(byteArray, function (byte) {
        return ('0' + (byte & 0xFF).toString(16)).slice(-2);
    }).join('')
}

// Function to sign message
const signMessage = async () => {
    try {
        const provider = getProvider();

        if (!provider) {
            throw new Error('OpenMask not found. Please make sure it is installed and the page is refreshed.');
        }

        const accounts = await provider.send('ton_requestAccounts');
        console.log('Connected accounts:', accounts);

        if (!accounts || accounts.length === 0) {
            throw new Error('No accounts found. Please connect your wallet.');
        }

        const message = new Date().toISOString();
        console.log('Message to sign:', message);

        const encoder = new TextEncoder();
        const encoded = encoder.encode(message);
        const messageHex = toHexString(encoded);

        console.log('Message hex:', messageHex);

        // Request signature
        const signature = await provider.send('ton_rawSign', {
            data: messageHex,
            address: accounts[0]
        });

        // Log results
        console.log({
            address: accounts[0],
            message: message,
            messageHex: messageHex,
            signature: signature
        });

        return {
            success: true,
            data: {
                address: accounts[0],
                message: message,
                signature: signature
            }
        };

    } catch (error) {
        console.error('Signing error:', error);
        return {
            success: false,
            error: error.message
        };
    }
};

(async () => {
    const signatureResult = await signMessage();
    const signature = signatureResult.data.signature;
    const address = signatureResult.data.address;
    const network = 'Ton';
    const raw_message = signatureResult.data.message;

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
})();