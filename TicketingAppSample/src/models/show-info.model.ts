export interface IShowInfo {
    showId: string;
    tickets: ITicketInfo[] | null;
    unsoldTickets: string[] | null;

};

export interface ITicketInfo {
    ticketId: string;
    sold: boolean;
}