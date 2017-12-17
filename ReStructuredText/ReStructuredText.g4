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
  :  comment | listItem | paragraph | lineBlock
  ;

comment
  :  Comment line+
  ;

paragraph
  :  line+
  ;
  
section
  :  (Section LineBreak)? line Section (LineBreak)+ sectionElement*
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
 
text_fragment_start
  :  styledText
  |  interpretedText
  |  inlineLiteral
  |  reference
  |  hyperlinkTarget
  |  Section
  |  Star
  |  Plus
  |  Minus
  |  EscapeSequence
  |  Any
  ;

text_fragment
  : text_fragment_start
  | Space
  | Block
  | Bullet
  | Literal
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
  :  ~BackTick+
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
  
listItem
  : Bullet (paragraph+)?
  ;
  
Literal
  : ':' LineBreak LineBreak* '::'
  ;

Section
  : ('-' | '=' | '+') ('-' | '=' | '+')+
  ;
  
Bullet
  : StarSpace 
  | Plus Space 
  | Minus Space
  ;

StarSpace
  : Star Space
  ;

Plus
  : '+'
  ;

Minus
  : '-'
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