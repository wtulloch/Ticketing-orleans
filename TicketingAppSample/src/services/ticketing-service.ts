import {get, post} from './http-service';
import {IShowInfo, ITicketInfo} from '../models/show-info.model';

export class TicketingServices {
    private endPointBaseUrl = 'http://127.0.0.1:5001/api/ticketing/';

    async getShows(): Promise<string[] | undefined> {
        const requestUrl = `${this.endPointBaseUrl}showinfo`;
        try {
            let result = await get<string[]>(requestUrl);
            
            console.log(result.parsedBody);
            
            return result.parsedBody;
        } catch (error) {
            console.error("get shows failed", error);
            return ['No shows found'];
        }
    }
   async getAvailableTickets(showId: string): Promise<IShowInfo | undefined> {
        const requestUrl = `${this.endPointBaseUrl}ticketsunreserved/${showId}`;
        
        try {
            let result = await get<IShowInfo>(requestUrl);
            return result.parsedBody;
        } catch (error) {
            console.error('failed to get show info',error);
            return;
        }
    }

    async getAllTickets(showId: string): Promise<IShowInfo | undefined> {
        const requestUrl = `${this.endPointBaseUrl}tickets/${showId}`;

        try {
            let result = await get<IShowInfo>(requestUrl);
            return result.parsedBody;
        } catch (error) {
            console.error('failed to get all tickets',error);
            return;
        }
    }

    async bookTicket(showId: string, ticketId: string): Promise<boolean> {
        const requestUrl = `${this.endPointBaseUrl}${showId}`;
        const newTicket : ITicketInfo = {
            ticketId: ticketId,
            sold: true
        };

        try {
             await post<ITicketInfo>(requestUrl, newTicket)
            return true;

        } catch (error) {
            console.error("Failed to book ticket", error);
            return false;
        }
    }
}
