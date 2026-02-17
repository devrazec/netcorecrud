import { useEffect } from 'react';
import { useRefresh } from 'react-admin';
import * as signalR from '@microsoft/signalr';

// Create SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5284/hubs/products')
    .withAutomaticReconnect()
    .build();

// Start connection
connection.start()
    .then(() => console.log('✅ SignalR Connected - Realtime updates enabled'))
    .catch(err => console.error('❌ SignalR Connection Error: ', err));

// Hook to enable realtime updates
export const useRealtime = () => {
    const refresh = useRefresh();

    useEffect(() => {
        const handler = () => {
            console.log('📡 Received update from server, refreshing...');
            refresh();
        };

        // Listen for productsChanged events
        connection.on('productsChanged', handler);

        // Cleanup on unmount
        return () => {
            connection.off('productsChanged', handler);
        };
    }, [refresh]);
};
