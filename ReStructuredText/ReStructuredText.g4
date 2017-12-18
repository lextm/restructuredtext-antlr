grammar ReStructuredText;

/* https://stackoverflow.com/questions/6178546/antlr-grammar-for-restructuredtext-rule-priorities */

options {
  language=CSharp;
}

parse
  :  (element | empty_line) + EOF
  ;

element
  :  section | sectionElement
  ;
  
sectionElement
  :  comment | listItemBullet | paragraph | listItemEnumerated | lineBlock
  ;

comment
  :  Space* Comment Space* LineBreak? (line+)?
  ;

paragraph
  :  line+
  ;
  
section
  :  (Section LineBreak)? title Section (LineBreak)+ sectionElement*
  ;
  
lineBlock
  :  (Block line)+
  ;  

line
  :  indentation? text
  ;

empty_line
  :  Space* LineBreak
  ;

indentation
  :  Space+
  ;

text
  : text_fragment_start text_fragment* LineBreak
  ;
  
title
  : text_fragment+ LineBreak
  ;
 
text_fragment_start
  :  styledText
  |  interpretedText
  |  inlineLiteral
  |  reference
  |  hyperlinkTarget
  |  hyperlink
  |  hyperlinkToc
  |  hyperlinkDoc
  |  url
  |  Section
  |  Star
  |  Plus
  |  Minus
  |  Colon
  |  EscapeSequence
  |  UnderScore
  |  DIGITS
  |  STRING
  |  '/'
  |  '.'
  |  '#'
  |  '['
  |  ']'
  |  '('
  |  ')'
  |  ';'
  |  '='
  |  '?'
  |  '<'
  |  '>'
  |  '&'
  |  Any
  ;

text_fragment
  :  text_fragment_start
  |  Space
  |  Block
  |  Bullet
  |  Literal
  |  Enumerated
  ;

styledText
  :  bold
  |  italic
  ;

bold
  :  Star Star boldAtom+ Star Star
  ;  

italic
  :  Star italicAtom+ (Star | StarSpace)
  ;

boldAtom
  :  ~Star | ~LineBreak
  |  italic
  ;

italicAtom
  :  ~Star | ~LineBreak
  |  bold
  ;

interpretedText
  :  BackTick interpretedTextAtoms BackTick
  ;

interpretedTextAtoms
  :  ~(BackTick | '<' | '>')+
  ;

inlineLiteral
  :  BackTick BackTick inlineLiteralAtoms BackTick BackTick
  ;

inlineLiteralAtoms
  :  inlineLiteralAtom+
  ;

inlineLiteralAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

hyperlinkTarget
  :  UnderScore Any+
  ;
  
hyperlink
  :  BackTick hyperlinkAtom+ Space '<' url '>' BackTick UnderScore
  ;
  
hyperlinkToc
  :  hyperlinkAtom+ Space '<' url '>'
  ;
  
hyperlinkDoc
  :  ':doc:' BackTick hyperlinkAtom+ Space '<' url '>' BackTick
  |  ':doc:' BackTick url BackTick
  ;
  
hyperlinkAtom
  :  ~( LineBreak | '<' | '>' | '`' | '*' )
  ;

listItemBullet
  :  Space* Bullet (paragraph+)?
  ;

listItemEnumerated
  :  Enumerated paragraph+ ending=LineBreak? 
  ;
  
url
   : relativeUri
   | absoluteUri
   ;

relativeUri
   : '/'? path '/' path
   ;

absoluteUri
   : scheme '://' login? host (':' port)? ('/' path )? '/'? query? frag?
   ;

scheme
   : string
   ;

host
   : '/'? (hostname | hostnumber)
   ;

hostname
   : string ('.' string)*
   ;

hostnumber
   : DIGITS '.' DIGITS '.' DIGITS '.' DIGITS
   ;

port
   : DIGITS
   ;

path
   : string ('/' string)*
   ;

user
   : string
   ;

login
   : user ':' password '@'
   ;

password
   : string
   ;

frag
   : ('#' string)
   ;

query
   : ('?' search)
   ;

search
   : searchparameter ('&' searchparameter)*
   ;

searchparameter
   : string ('=' (string | DIGITS | HEX))?
   ;

string
   : STRING | DIGITS
   ;

DIGITS
   : [0-9] +
   ;


HEX
   : ('%' [a-fA-F0-9] [a-fA-F0-9]) +
   ;


STRING
   : ([a-zA-Z~0-9] | HEX) ([a-zA-Z0-9.-] | HEX | '_' | '+')*
   ;


Colon
  :  ':'
  ;


Literal
  :  Colon LineBreak LineBreak* Colon Colon
  ;

Section
  :  ('-' | '=' | '+') ('-' | '=' | '+')+
  ;
  
Bullet
  :  StarSpace 
  |  Plus Space 
  |  Minus Space
  ;
  
Enumerated
  :  (DIGITS '.' Space)
  ;

StarSpace
  :  Star Space
  ;

Plus
  :  '+'
  ;

Minus
  :  '-'
  ;

Block
  :  '| '
  ;

Comment
  :  '..'
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | '*')
  ;

LineBreak
  :  '\r'? '\n'
  ;

Any
  :  .
  ;