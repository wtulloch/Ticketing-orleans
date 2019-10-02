import {get, post} from './http-service';
import {IShowData, ITicketStatus, IShowInformation} from '../models/show-info.model';

export class TicketingServices {
    private endPointBaseUrl = 'http://127.0.0.1:5001/api/ticketing/';

    async getShows(): Promise<IShowInformation[] | undefined> {
        const requestUrl = `${this.endPointBaseUrl}showinfo`;
        try {
            let result = await get<IShowInformation[]>(requestUrl);
            
            console.log(result.parsedBody);
            
            return result.parsedBody;
        } catch (error) {
            console.error("get shows failed", error);
            return [];
        }
    }
   async getAvailableTickets(showId: string): Promise<IShowData | undefined> {
        const requestUrl = `${this.endPointBaseUrl}ticketsunreserved/${showId}`;
        
        try {
            let result = await get<IShowData>(requestUrl);
            return result.parsedBody;
        } catch (error) {
            console.error('failed to get show info',error);
            return;
        }
    }

    async getAllTickets(showId: string,date: string): Promise<IShowData | undefined> {
        const requestUrl = `${this.endPointBaseUrl}tickets/${showId}?date=${date}`;

        try {
            let result = await get<IShowData>(requestUrl);
            return result.parsedBody;
        } catch (error) {
            console.error('failed to get all tickets',error);
            return;
        }
    }

    async bookTicket(showId: string, ticketId: string): Promise<boolean> {
        const requestUrl = `${this.endPointBaseUrl}${showId}`;
        const newTicket : ITicketStatus = {
            ticketId: ticketId,
            sold: true
        };

        try {
             await post<ITicketStatus>(requestUrl, newTicket)
            return true;

        } catch (error) {
            console.error("Failed to book ticket", error);
            return false;
        }
    }
}
