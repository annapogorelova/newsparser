export class HttpError extends Error {
    public status: number;
    
    constructor(message: string, status: number) {
        super(message);
        this.message = message;
        this.name = 'HttpError';
        this.status = status;
    }
}